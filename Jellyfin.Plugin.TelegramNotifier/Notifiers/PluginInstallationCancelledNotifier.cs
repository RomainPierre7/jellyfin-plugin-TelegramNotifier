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

        string message = $"🔴 {eventArgs.Argument.Name} plugin installation cancelled (version {eventArgs.Argument.Version}):";

        await _notificationFilter.Filter(NotificationFilter.NotificationType.PluginInstallationCancelled, message).ConfigureAwait(false);
    }
}