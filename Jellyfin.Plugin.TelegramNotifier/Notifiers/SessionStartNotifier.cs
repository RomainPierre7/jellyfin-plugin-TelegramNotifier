using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using MediaBrowser.Controller.Events;
using MediaBrowser.Controller.Events.Session;

namespace Jellyfin.Plugin.TelegramNotifier.Notifiers;

public class SessionStartNotifier : IEventConsumer<SessionStartedEventArgs>
{
    private readonly NotificationFilter _notificationFilter;
    private static readonly ConcurrentDictionary<string, DateTime> _recentEvents = new();
    private static readonly TimeSpan RecentEventThreshold = TimeSpan.FromSeconds(5);
    private static readonly TimeSpan CleanupThreshold = TimeSpan.FromMinutes(5);

    public SessionStartNotifier(NotificationFilter notificationFilter)
    {
        _notificationFilter = notificationFilter;
    }

    public async Task OnEvent(SessionStartedEventArgs eventArgs)
    {
        if (eventArgs == null)
        {
            throw new ArgumentNullException(nameof(eventArgs));
        }

        // Clean up old session entries when a new session event is triggered
        CleanupOldEntries();

        // Generate a unique key for this session event
        string sessionKey = eventArgs.Argument.Id;

        // Check if we've processed a similar event recently
        if (_recentEvents.TryGetValue(sessionKey, out DateTime lastProcessedTime) &&
            DateTime.UtcNow - lastProcessedTime < RecentEventThreshold)
        {
            return;
        }

        // Update the cache with the latest event time
        _recentEvents[sessionKey] = DateTime.UtcNow;

        string userId = eventArgs.Argument.UserId.ToString();

        await _notificationFilter.Filter(NotificationFilter.NotificationType.SessionStart, eventArgs, userId).ConfigureAwait(false);
    }

    private static void CleanupOldEntries()
    {
        DateTime threshold = DateTime.UtcNow - CleanupThreshold;
        var keysToRemove = _recentEvents
            .Where(kvp => kvp.Value < threshold)
            .Select(kvp => kvp.Key)
            .ToList();

        foreach (var key in keysToRemove)
        {
            _recentEvents.TryRemove(key, out _);
        }
    }
}