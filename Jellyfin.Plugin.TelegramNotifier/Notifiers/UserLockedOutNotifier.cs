using System;
using System.Threading.Tasks;
using Jellyfin.Data.Events.Users;
using MediaBrowser.Controller.Events;

namespace Jellyfin.Plugin.TelegramNotifier.Notifiers;

public class UserLockedOutNotifier : IEventConsumer<UserLockedOutEventArgs>
{
    private readonly NotificationFilter _notificationFilter;

    public UserLockedOutNotifier(NotificationFilter notificationFilter)
    {
        _notificationFilter = notificationFilter;
    }

    public async Task OnEvent(UserLockedOutEventArgs eventArgs)
    {
        if (eventArgs == null)
        {
            throw new ArgumentNullException(nameof(eventArgs));
        }

        string message = $"👤🔒 User {eventArgs.Argument.Username} locked out";

        await _notificationFilter.Filter(NotificationFilter.NotificationType.UserLockedOut, message).ConfigureAwait(false);
    }
}