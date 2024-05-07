using System;
using System.Threading.Tasks;
using Jellyfin.Data.Events.Users;
using MediaBrowser.Controller.Events;

namespace Jellyfin.Plugin.TelegramNotifier.Notifiers;

public class UserUpdatedNotifier : IEventConsumer<UserUpdatedEventArgs>
{
    private readonly NotificationFilter _notificationFilter;

    public UserUpdatedNotifier(NotificationFilter notificationFilter)
    {
        _notificationFilter = notificationFilter;
    }

    public async Task OnEvent(UserUpdatedEventArgs eventArgs)
    {
        if (eventArgs == null)
        {
            throw new ArgumentNullException(nameof(eventArgs));
        }

        string message = $"👤 User {eventArgs.Argument.Username} has been updated";

        await _notificationFilter.Filter(NotificationFilter.NotificationType.UserUpdated, message).ConfigureAwait(false);
    }
}