using System;
using System.Threading.Tasks;
using Jellyfin.Data.Events;
using MediaBrowser.Controller.Authentication;
using MediaBrowser.Controller.Events;

namespace Jellyfin.Plugin.TelegramNotifier.Notifiers;

public class AuthenticationSuccessNotifier : IEventConsumer<GenericEventArgs<AuthenticationResult>>
{
    private readonly NotificationFilter _notificationFilter;

    public AuthenticationSuccessNotifier(NotificationFilter notificationFilter)
    {
        _notificationFilter = notificationFilter;
    }

    public async Task OnEvent(GenericEventArgs<AuthenticationResult> eventArgs)
    {
        if (eventArgs == null)
        {
            throw new ArgumentNullException(nameof(eventArgs));
        }

        string message = $"🔓 Authentication success for user {eventArgs.Argument.User.Name} on {eventArgs.Argument.SessionInfo.DeviceName}";

        await _notificationFilter.Filter(NotificationFilter.NotificationType.AuthenticationSuccess, message).ConfigureAwait(false);
    }
}