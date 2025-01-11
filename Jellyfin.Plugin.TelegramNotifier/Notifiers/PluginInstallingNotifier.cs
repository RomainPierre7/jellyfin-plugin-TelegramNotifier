using System;
using System.Threading.Tasks;
using MediaBrowser.Controller.Events;
using MediaBrowser.Controller.Events.Updates;

namespace Jellyfin.Plugin.TelegramNotifier.Notifiers;

public class PluginInstallingNotifier : IEventConsumer<PluginInstallingEventArgs>
{
    private readonly NotificationFilter _notificationFilter;

    public PluginInstallingNotifier(NotificationFilter notificationFilter)
    {
        _notificationFilter = notificationFilter;
    }

    public async Task OnEvent(PluginInstallingEventArgs eventArgs)
    {
        if (eventArgs == null)
        {
            throw new ArgumentNullException(nameof(eventArgs));
        }

        await _notificationFilter.Filter(NotificationFilter.NotificationType.PluginInstalling, eventArgs).ConfigureAwait(false);
    }
}