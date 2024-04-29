using System;
using System.Threading.Tasks;
using MediaBrowser.Controller.Events;
using MediaBrowser.Controller.Events.Updates;

namespace Jellyfin.Plugin.TelegramNotifier.Notifiers;

public class PluginUpdatedNotifier : IEventConsumer<PluginUpdatedEventArgs>
{
    private readonly NotificationFilter _notificationFilter;

    public PluginUpdatedNotifier(NotificationFilter notificationFilter)
    {
        _notificationFilter = notificationFilter;
    }

    public async Task OnEvent(PluginUpdatedEventArgs eventArgs)
    {
        if (eventArgs == null)
        {
            throw new ArgumentNullException(nameof(eventArgs));
        }

        string message = $"🚧 {eventArgs.Argument.Name} plugin updated to version {eventArgs.Argument.Version}:" +
                         $"🗒️ {eventArgs.Argument.Changelog}";

        await _notificationFilter.Filter(NotificationFilter.NotificationType.PluginUpdated, message).ConfigureAwait(false);
    }
}