using System;
using System.Globalization;
using System.Threading.Tasks;
using MediaBrowser.Controller.Entities.TV;
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

        if (eventArgs.Item.GetType() == typeof(MediaBrowser.Controller.Entities.Audio.Audio))
        {
            return;
        }

        string message = $"👤 {eventArgs.Users[0].Username} is still watching on {eventArgs.DeviceName}:\n" +
                         $"🎬 {eventArgs.Item.Name} ({eventArgs.Item.ProductionYear})";

        switch (eventArgs.Item)
        {
            case Episode episode:
                string seasonNumber = episode.Season.IndexNumber.HasValue ? episode.Season.IndexNumber.Value.ToString("00", CultureInfo.InvariantCulture) : "00";
                string episodeNumber = episode.IndexNumber.HasValue ? episode.IndexNumber.Value.ToString("00", CultureInfo.InvariantCulture) : "00";

                message = $"👤 {eventArgs.Users[0].Username} is still watching on {eventArgs.DeviceName}:\n" +
                          $"🎬 {episode.Series.Name} ({eventArgs.Item.ProductionYear})\n" +
                          $"      S{seasonNumber} - E{episodeNumber}\n" +
                          $"      '{eventArgs.Item.Name}'";
                break;
        }

        string userId = eventArgs.Users[0].Id.ToString();

        await _notificationFilter.Filter(NotificationFilter.NotificationType.PlaybackProgress, message, userId: userId).ConfigureAwait(false);
    }
}