using System;
using System.Threading.Tasks;
using MediaBrowser.Controller.Events;
using MediaBrowser.Controller.Library;

namespace Jellyfin.Plugin.TelegramNotifier.Notifiers;

public class PlaybackProgressNotifier : IEventConsumer<PlaybackProgressEventArgs>
{
    private readonly NotificationFilter _notificationFilter;

    public PlaybackProgressNotifier(NotificationFilter notificationFilter)
    {
        _notificationFilter = notificationFilter;
    }

    public async Task OnEvent(PlaybackProgressEventArgs eventArgs)
    {
        if (eventArgs == null)
        {
            throw new ArgumentNullException(nameof(eventArgs));
        }

        if (eventArgs.Item is null || eventArgs.Users.Count == 0 || eventArgs.Item.IsThemeMedia)
        {
            return;
        }

        string message = $"👤 {eventArgs.Users[0].Username} is still watching on {eventArgs.DeviceName}:\n" +
                         $"🎬 {eventArgs.Item.Name} ({eventArgs.Item.ProductionYear})";

        await _notificationFilter.Filter(NotificationFilter.NotificationType.PlaybackProgress, message).ConfigureAwait(false);
    }
}