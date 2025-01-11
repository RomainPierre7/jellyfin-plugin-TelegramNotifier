using System;
using System.Threading.Tasks;
using MediaBrowser.Common.Updates;
using MediaBrowser.Controller.Events;

namespace Jellyfin.Plugin.TelegramNotifier.Notifiers;

public class PluginInstallationFailedNotifier : IEventConsumer<InstallationFailedEventArgs>
{
    private readonly NotificationFilter _notificationFilter;

    public PluginInstallationFailedNotifier(NotificationFilter notificationFilter)
    {
        _notificationFilter = notificationFilter;
    }

    public async Task OnEvent(InstallationFailedEventArgs eventArgs)
    {
        if (eventArgs == null)
        {
            throw new ArgumentNullException(nameof(eventArgs));
        }

        await _notificationFilter.Filter(NotificationFilter.NotificationType.PluginInstallationFailed, eventArgs).ConfigureAwait(false);
    }
}