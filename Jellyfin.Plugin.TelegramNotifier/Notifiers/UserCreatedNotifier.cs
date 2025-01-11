using System;
using System.Threading.Tasks;
using Jellyfin.Data.Events.Users;
using MediaBrowser.Controller.Events;

namespace Jellyfin.Plugin.TelegramNotifier.Notifiers;

public class UserCreatedNotifier : IEventConsumer<UserCreatedEventArgs>
{
    private readonly NotificationFilter _notificationFilter;

    public UserCreatedNotifier(NotificationFilter notificationFilter)
    {
        _notificationFilter = notificationFilter;
    }

    public async Task OnEvent(UserCreatedEventArgs eventArgs)
    {
        if (eventArgs == null)
        {
            throw new ArgumentNullException(nameof(eventArgs));
        }

        await _notificationFilter.Filter(NotificationFilter.NotificationType.UserCreated, eventArgs).ConfigureAwait(false);
    }
}