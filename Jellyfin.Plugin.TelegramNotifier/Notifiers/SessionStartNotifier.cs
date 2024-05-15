using System;
using System.Threading.Tasks;
using MediaBrowser.Controller.Events;
using MediaBrowser.Controller.Events.Session;

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

        string message = $"👤 {eventArgs.Argument.UserName} has started a session on:\n" +
                         $"💻 {eventArgs.Argument.Client} ({eventArgs.Argument.DeviceName})\n";

        string userId = eventArgs.Argument.UserId.ToString();

        await _notificationFilter.Filter(NotificationFilter.NotificationType.SessionStart, message, userId).ConfigureAwait(false);
    }
}