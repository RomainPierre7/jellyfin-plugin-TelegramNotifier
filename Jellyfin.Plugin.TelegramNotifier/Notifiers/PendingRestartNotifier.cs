using System;
using System.Threading.Tasks;
using Jellyfin.Data.Events.System;
using MediaBrowser.Controller.Events;

namespace Jellyfin.Plugin.TelegramNotifier.Notifiers;

public class PendingRestartNotifier : IEventConsumer<PendingRestartEventArgs>
{
    private readonly NotificationFilter _notificationFilter;

    public PendingRestartNotifier(NotificationFilter notificationFilter)
    {
        _notificationFilter = notificationFilter;
    }

    public async Task OnEvent(PendingRestartEventArgs eventArgs)
    {
        if (eventArgs == null)
        {
            throw new ArgumentNullException(nameof(eventArgs));
        }

        string message = $"🔄 Jellyfin is pending a restart.";

        await _notificationFilter.Filter(NotificationFilter.NotificationType.PendingRestart, message).ConfigureAwait(false);
    }
}