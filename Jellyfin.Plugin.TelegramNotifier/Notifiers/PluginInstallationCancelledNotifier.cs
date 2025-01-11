using System;
using System.Threading.Tasks;
using MediaBrowser.Controller.Events;
using MediaBrowser.Controller.Events.Updates;

namespace Jellyfin.Plugin.TelegramNotifier.Notifiers;

public class PluginInstallationCancelledNotifier : IEventConsumer<PluginInstallationCancelledEventArgs>
{
    private readonly NotificationFilter _notificationFilter;

    public PluginInstallationCancelledNotifier(NotificationFilter notificationFilter)
    {
        _notificationFilter = notificationFilter;
    }

    public async Task OnEvent(PluginInstallationCancelledEventArgs eventArgs)
    {
        if (eventArgs == null)
        {
            throw new ArgumentNullException(nameof(eventArgs));
        }

        await _notificationFilter.Filter(NotificationFilter.NotificationType.PluginInstallationCancelled, eventArgs).ConfigureAwait(false);
    }
}