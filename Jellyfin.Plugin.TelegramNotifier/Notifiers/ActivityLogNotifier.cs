using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Jellyfin.Data.Events;
using MediaBrowser.Controller;
using MediaBrowser.Controller.Library;
using MediaBrowser.Model.Activity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.TelegramNotifier.Notifiers;

/// <summary>
/// Forwards entries of Jellyfin's native Activity Log to Telegram, so other plugins only need to write an
/// activity entry instead of keeping their own bot credentials.
/// </summary>
public class ActivityLogNotifier : IHostedService
{
    private static readonly Regex ListSeparator = new(@"[,;\r\n]+", RegexOptions.Compiled);

    private readonly IActivityManager _activityManager;
    private readonly IUserManager _userManager;
    private readonly IServerApplicationHost _applicationHost;
    private readonly ILogger<ActivityLogNotifier> _logger;

    public ActivityLogNotifier(
        IActivityManager activityManager,
        IUserManager userManager,
        IServerApplicationHost applicationHost,
        ILogger<ActivityLogNotifier> logger)
    {
        _activityManager = activityManager;
        _userManager = userManager;
        _applicationHost = applicationHost;
        _logger = logger;
    }

    /// <summary>
    /// Decides whether an activity type passes a user's include/exclude lists.
    /// Empty include list = everything. Exclusions win. Case-insensitive, "*" wildcard.
    /// </summary>
    /// <param name="includeTypes">Include list (comma, semicolon or newline separated).</param>
    /// <param name="excludeTypes">Exclude list (same format).</param>
    /// <param name="activityType">The activity type of the entry.</param>
    /// <returns>True if the entry should be notified.</returns>
    public static bool IsTypeAllowed(string? includeTypes, string? excludeTypes, string? activityType)
    {
        var type = activityType ?? string.Empty;
        var excluded = Split(excludeTypes);
        if (excluded.Any(p => Matches(p, type)))
        {
            return false;
        }

        var included = Split(includeTypes);
        return included.Count == 0 || included.Any(p => Matches(p, type));
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _activityManager.EntryCreated += OnEntryCreated;
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _activityManager.EntryCreated -= OnEntryCreated;
        return Task.CompletedTask;
    }

    private static List<string> Split(string? list)
    {
        return string.IsNullOrWhiteSpace(list)
            ? new List<string>()
            : ListSeparator.Split(list).Select(x => x.Trim()).Where(x => x.Length > 0).ToList();
    }

    private static bool Matches(string pattern, string value)
    {
        if (!pattern.Contains('*', StringComparison.Ordinal))
        {
            return string.Equals(pattern, value, StringComparison.OrdinalIgnoreCase);
        }

        var regex = "^" + Regex.Escape(pattern).Replace("\\*", ".*", StringComparison.Ordinal) + "$";
        return Regex.IsMatch(value, regex, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
    }

    private async void OnEntryCreated(object? sender, GenericEventArgs<ActivityLogEntry> e)
    {
        try
        {
            if (!Plugin.Config.EnablePlugin || e?.Argument is null)
            {
                return;
            }

            // Cheap exit: nobody opted in.
            if (!Plugin.Config.UserConfigurations.Any(u => u.EnableUser && u.ActivityLog))
            {
                return;
            }

            var entry = e.Argument;
            var userName = string.Empty;
            if (entry.UserId != Guid.Empty)
            {
                userName = _userManager.GetUserById(entry.UserId)?.Username ?? string.Empty;
            }

            var scope = _applicationHost.ServiceProvider!.CreateAsyncScope();
            await using (scope.ConfigureAwait(false))
            {
                var filter = scope.ServiceProvider.GetRequiredService<NotificationFilter>();
                await filter.Filter(
                    NotificationFilter.NotificationType.ActivityLog,
                    new ActivityNotificationArgs(entry, userName),
                    entry.UserId.ToString("N", CultureInfo.InvariantCulture)).ConfigureAwait(false);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "TelegramNotifier: error while forwarding an activity log entry.");
        }
    }
}
