using System;
using System.Threading.Tasks;
using MediaBrowser.Controller.Events;
using MediaBrowser.Controller.Events.Session;
using MediaBrowser.Controller.Library;

namespace Jellyfin.Plugin.TelegramNotifier.Notifiers;

public class SessionStartNotifier : IEventConsumer<SessionStartedEventArgs>
{
    private readonly NotificationFilter _notificationFilter;

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

        string message = $"Session started: {eventArgs.Argument.Client} ({eventArgs.Argument.DeviceName})\n" +
                         $"Item: {eventArgs.Argument.FullNowPlayingItem}";

        await _notificationFilter.Filter(NotificationFilter.NotificationType.SessionStart, message).ConfigureAwait(false);
    }
}