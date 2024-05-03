using System;
using System.Threading.Tasks;
using MediaBrowser.Controller.Events;
using MediaBrowser.Controller.Subtitles;

namespace Jellyfin.Plugin.TelegramNotifier.Notifiers;

public class SubtitleDownloadFailureNotifier : IEventConsumer<SubtitleDownloadFailureEventArgs>
{
    private readonly NotificationFilter _notificationFilter;

    public SubtitleDownloadFailureNotifier(NotificationFilter notificationFilter)
    {
        _notificationFilter = notificationFilter;
    }

    public async Task OnEvent(SubtitleDownloadFailureEventArgs eventArgs)
    {
        if (eventArgs == null)
        {
            throw new ArgumentNullException(nameof(eventArgs));
        }

        string message = $"🚫 Subtitle download failed for {eventArgs.Item.Name}";

        await _notificationFilter.Filter(NotificationFilter.NotificationType.SubtitleDownloadFailure, message).ConfigureAwait(false);
    }
}