using System;
using System.Threading.Tasks;
using MediaBrowser.Controller.Events;
using MediaBrowser.Controller.Events.Updates;

namespace Jellyfin.Plugin.TelegramNotifier.Notifiers;

public class PluginUninstalledNotifier : IEventConsumer<PluginUninstalledEventArgs>
{
    private readonly NotificationFilter _notificationFilter;

    public PluginUninstalledNotifier(NotificationFilter notificationFilter)
    {
        _notificationFilter = notificationFilter;
    }

    public async Task OnEvent(PluginUninstalledEventArgs eventArgs)
    {
        if (eventArgs == null)
        {
            throw new ArgumentNullException(nameof(eventArgs));
        }

        string message = $"🚧 {eventArgs.Argument.Name} plugin uninstalled";

        await _notificationFilter.Filter(NotificationFilter.NotificationType.PluginUninstalled, message).ConfigureAwait(false);
    }
}