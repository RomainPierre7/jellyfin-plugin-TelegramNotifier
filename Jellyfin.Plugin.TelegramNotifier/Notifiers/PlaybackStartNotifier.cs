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

        string message = $"👤 {eventArgs.Users[0].Username} is watching on {eventArgs.DeviceName}:\n" +
                         $"🎬 {eventArgs.Item.Name} ({eventArgs.Item.ProductionYear})\n" +
                         $"📺 [{eventArgs.Item.MediaType}] {string.Join(", ", eventArgs.Item.Genres)}\n" +
                         $"🕒 {eventArgs.Item.RunTimeTicks / 600000000} minutes\n" +
                         $"📽 {eventArgs.Item.Overview}";

        switch (eventArgs.Item)
        {
            case Episode episode:
                string seasonNumber = episode.Season.IndexNumber.HasValue ? episode.Season.IndexNumber.Value.ToString("00", CultureInfo.InvariantCulture) : "00";
                string episodeNumber = episode.IndexNumber.HasValue ? episode.IndexNumber.Value.ToString("00", CultureInfo.InvariantCulture) : "00";

                message = $"👤 {eventArgs.Users[0].Username} is watching on {eventArgs.DeviceName}:\n" +
                          $"🎬 {episode.Series.Name} ({eventArgs.Item.ProductionYear}) | S{seasonNumber} - E{episodeNumber}\n" +
                          $"      {eventArgs.Item.Name}\n" +
                          $"📺 [{eventArgs.Item.MediaType}] {string.Join(", ", eventArgs.Item.Genres)}\n" +
                          $"🕒 {eventArgs.Item.RunTimeTicks / 600000000} minutes\n" +
                          $"📽 {eventArgs.Item.Overview}";
                break;
        }

        await _notificationFilter.Filter(NotificationFilter.NotificationType.PlaybackStart, message).ConfigureAwait(false);
    }
}