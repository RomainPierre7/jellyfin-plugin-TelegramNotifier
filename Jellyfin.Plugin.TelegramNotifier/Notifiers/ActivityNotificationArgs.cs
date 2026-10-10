using System;
using System.Collections.Generic;
using System.Globalization;
using MediaBrowser.Model.Activity;

namespace Jellyfin.Plugin.TelegramNotifier.Notifiers;

/// <summary>
/// Event payload for activity log notifications. Exposes the {activity.*} placeholders.
/// </summary>
public class ActivityNotificationArgs
{
    public ActivityNotificationArgs(ActivityLogEntry entry, string userName)
    {
        Entry = entry;
        UserName = userName;
    }

    public ActivityLogEntry Entry { get; }

    public string UserName { get; }

    public string Type => Entry.Type ?? string.Empty;

    /// <summary>
    /// Placeholder values, HTML-escaped because messages are sent with Telegram's HTML parse mode
    /// and activity text can contain user supplied content (e.g. title requests).
    /// </summary>
    /// <returns>The placeholder map.</returns>
    public Dictionary<string, string?> GetReplacements()
    {
        static string Enc(string? v) => (v ?? string.Empty)
            .Replace("&", "&amp;", StringComparison.Ordinal)
            .Replace("<", "&lt;", StringComparison.Ordinal)
            .Replace(">", "&gt;", StringComparison.Ordinal);

        return new Dictionary<string, string?>
        {
            { "{activity.Name}", Enc(Entry.Name) },
            { "{activity.ShortOverview}", Enc(Entry.ShortOverview) },
            { "{activity.Overview}", Enc(Entry.Overview) },
            { "{activity.Type}", Enc(Entry.Type) },
            { "{activity.Severity}", Entry.Severity.ToString() },
            { "{activity.Date}", Entry.Date.ToLocalTime().ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture) },
            { "{activity.UserName}", Enc(UserName) },
            { "{activity.ItemId}", Entry.ItemId },
        };
    }
}
