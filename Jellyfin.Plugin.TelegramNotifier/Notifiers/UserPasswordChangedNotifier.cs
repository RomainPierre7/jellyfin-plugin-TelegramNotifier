using System;
using System.Threading.Tasks;
using Jellyfin.Data.Events.Users;
using MediaBrowser.Controller.Events;

namespace Jellyfin.Plugin.TelegramNotifier.Notifiers;

public class UserPasswordChangedNotifier : IEventConsumer<UserPasswordChangedEventArgs>
{
    private readonly NotificationFilter _notificationFilter;

    public UserPasswordChangedNotifier(NotificationFilter notificationFilter)
    {
        _notificationFilter = notificationFilter;
    }

    public async Task OnEvent(UserPasswordChangedEventArgs eventArgs)
    {
        if (eventArgs == null)
        {
            throw new ArgumentNullException(nameof(eventArgs));
        }

        await _notificationFilter.Filter(NotificationFilter.NotificationType.UserPasswordChanged).ConfigureAwait(false);
    }
}