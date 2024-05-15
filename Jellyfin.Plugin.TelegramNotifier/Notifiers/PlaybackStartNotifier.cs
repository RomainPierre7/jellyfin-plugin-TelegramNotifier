using System;
using System.Globalization;
using System.Threading.Tasks;
using MediaBrowser.Controller.Entities.TV;
using MediaBrowser.Controller.Events;
using MediaBrowser.Controller.Library;

namespace Jellyfin.Plugin.TelegramNotifier.Notifiers;

public class PlaybackStartNotifier : IEventConsumer<PlaybackStartEventArgs>
{
    private readonly NotificationFilter _notificationFilter;

    public PlaybackStartNotifier(NotificationFilter notificationFilter)
    {
        _notificationFilter = notificationFilter;
    }

    public async Task OnEvent(PlaybackStartEventArgs eventArgs)
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

        long? ticks = eventArgs.Item.RunTimeTicks;
        long hours = ticks.HasValue ? ticks.Value / (600000000L * 60) : 0;
        long minutes = ticks.HasValue ? (ticks.Value / 600000000L) % 60 : 0;
        string duration = minutes < 10 ? $"{hours}h {minutes}m" : $"{hours}h 0{minutes}m";
        if (hours == 0)
        {
            duration = minutes > 1 ? $"{minutes} minutes" : $"{minutes} minute";
        }

        string message = $"👤 {eventArgs.Users[0].Username} is watching on {eventArgs.DeviceName}:\n" +
                         $"🎬 {eventArgs.Item.Name} ({eventArgs.Item.ProductionYear})\n" +
                         $"📺 [{eventArgs.Item.MediaType}] {string.Join(", ", eventArgs.Item.Genres)}\n" +
                         $"🕒 {duration}\n" +
                         $"📽 {eventArgs.Item.Overview}";

        switch (eventArgs.Item)
        {
            case Episode episode:
                string seasonNumber = episode.Season.IndexNumber.HasValue ? episode.Season.IndexNumber.Value.ToString("00", CultureInfo.InvariantCulture) : "??";
                string episodeNumber = episode.IndexNumber.HasValue ? episode.IndexNumber.Value.ToString("00", CultureInfo.InvariantCulture) : "??";

                message = $"👤 {eventArgs.Users[0].Username} is watching on {eventArgs.DeviceName}:\n" +
                                          $"🎬 {episode.Series.Name} ({eventArgs.Item.ProductionYear})\n" +
                                          $"      S{seasonNumber} - E{episodeNumber}\n" +
                                          $"      '{eventArgs.Item.Name}'\n" +
                                          $"📺 [{eventArgs.Item.MediaType}] {string.Join(", ", eventArgs.Item.Genres)}\n" +
                                          $"🕒 {duration}\n" +
                                          $"📽 {eventArgs.Item.Overview}";
                break;
        }

        string userId = eventArgs.Users[0].Id.ToString();

        if (eventArgs.Item.PrimaryImagePath is not null)
        {
            string serverUrl = Plugin.Instance?.Configuration.ServerUrl ?? "localhost:8096";
            string path = "http://" + serverUrl + "/Items/" + eventArgs.Item.Id + "/Images/Primary";

            await _notificationFilter.Filter(NotificationFilter.NotificationType.PlaybackStart, message, userId: userId, imagePath: path).ConfigureAwait(false);
        }
        else
        {
            await _notificationFilter.Filter(NotificationFilter.NotificationType.PlaybackStart, message, userId: userId).ConfigureAwait(false);
        }
    }
}