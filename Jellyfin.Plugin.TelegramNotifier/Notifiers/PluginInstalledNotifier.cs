using System;
using System.Threading.Tasks;
using MediaBrowser.Controller.Events;
using MediaBrowser.Controller.Events.Updates;

namespace Jellyfin.Plugin.TelegramNotifier.Notifiers;

public class PluginInstalledNotifier : IEventConsumer<PluginInstalledEventArgs>
{
    private readonly NotificationFilter _notificationFilter;

    public PluginInstalledNotifier(NotificationFilter notificationFilter)
    {
        _notificationFilter = notificationFilter;
    }

    public async Task OnEvent(PluginInstalledEventArgs eventArgs)
    {
        if (eventArgs == null)
        {
            throw new ArgumentNullException(nameof(eventArgs));
        }

        string message = $"🚧 {eventArgs.Argument.Name} plugin installed (version {eventArgs.Argument.Version})";

        await _notificationFilter.Filter(NotificationFilter.NotificationType.PluginInstalled, message).ConfigureAwait(false);
    }
}