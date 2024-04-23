using System;
using System.Threading.Tasks;
using Jellyfin.Data.Events;
using MediaBrowser.Controller.Events;
using MediaBrowser.Controller.Session;

namespace Jellyfin.Plugin.TelegramNotifier.Notifiers;

public class AuthenticationFailureNotifier : IEventConsumer<GenericEventArgs<AuthenticationRequest>>
{
    private readonly NotificationFilter _notificationFilter;

    public AuthenticationFailureNotifier(NotificationFilter notificationFilter)
    {
        _notificationFilter = notificationFilter;
    }

    public async Task OnEvent(GenericEventArgs<AuthenticationRequest> eventArgs)
    {
        if (eventArgs == null)
        {
            throw new ArgumentNullException(nameof(eventArgs));
        }

        string message = $"🔒 Authentication failure on {eventArgs.Argument.DeviceName} for user {eventArgs.Argument.Username}";

        await _notificationFilter.Filter(NotificationFilter.NotificationType.AuthenticationFailure, message).ConfigureAwait(false);
    }
}