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

        string subtype = "PlaybackStartMovies";

        switch (eventArgs.Item)
        {
            case Episode episode:
                subtype = "PlaybackStartEpisodes";
                break;
        }

        string userId = eventArgs.Users[0].Id.ToString();

        if (eventArgs.Item.PrimaryImagePath is not null)
        {
            string serverUrl = Plugin.Instance?.Configuration.ServerUrl ?? "localhost:8096";
            string path = "http://" + serverUrl + "/Items/" + eventArgs.Item.Id + "/Images/Primary";

            await _notificationFilter.Filter(NotificationFilter.NotificationType.PlaybackStart, eventArgs, userId: userId, imagePath: path, subtype: subtype).ConfigureAwait(false);
        }
        else
        {
            await _notificationFilter.Filter(NotificationFilter.NotificationType.PlaybackStart, eventArgs, userId: userId, subtype: subtype).ConfigureAwait(false);
        }
    }
}