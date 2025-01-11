/* using System;
using System.Threading.Tasks;
using MediaBrowser.Controller.Events;

namespace Jellyfin.Plugin.TelegramNotifier.Notifiers;

public class TemplateNotifier : IEventConsumer<TemplateEventArgs>
{
    private readonly NotificationFilter _notificationFilter;

    public TemplateNotifier(NotificationFilter notificationFilter)
    {
        _notificationFilter = notificationFilter;
    }

    public async Task OnEvent(TemplateEventArgs eventArgs)
    {
        if (eventArgs == null)
        {
            throw new ArgumentNullException(nameof(eventArgs));
        }

        await _notificationFilter.Filter(NotificationFilter.NotificationType.Template).ConfigureAwait(false);
    }
} */