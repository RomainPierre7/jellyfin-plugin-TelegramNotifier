using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Entities.TV;
using MediaBrowser.Controller.Library;
using Microsoft.Extensions.Hosting;

namespace Jellyfin.Plugin.TelegramNotifier.Notifiers.ItemAddedNotifier;

public class ItemAddedNotifierEntryPoint : IHostedService
{
    private static readonly TimeSpan ChildUpdateSuppressionWindow = TimeSpan.FromMinutes(10);
    private readonly IItemAddedManager _itemAddedManager;
    private readonly ILibraryManager _libraryManager;
    private readonly ConcurrentDictionary<Guid, DateTime> _recentUpdatedSeasons = new();
    private readonly ConcurrentDictionary<Guid, DateTime> _recentUpdatedSeries = new();

    public ItemAddedNotifierEntryPoint(
        IItemAddedManager itemAddedManager,
        ILibraryManager libraryManager)
    {
        _itemAddedManager = itemAddedManager;
        _libraryManager = libraryManager;
    }

    private static Guid GetEpisodeSeasonId(Episode episode)
    {
        if (episode.SeasonId != Guid.Empty)
        {
            return episode.SeasonId;
        }

        return episode.Season?.Id ?? Guid.Empty;
    }

    private static Guid GetEpisodeSeriesId(Episode episode)
    {
        if (episode.SeriesId != Guid.Empty)
        {
            return episode.SeriesId;
        }

        return episode.Series?.Id ?? Guid.Empty;
    }

    private static Guid GetSeasonSeriesId(Season season)
    {
        if (season.SeriesId != Guid.Empty)
        {
            return season.SeriesId;
        }

        return season.Series?.Id ?? Guid.Empty;
    }

    private static bool HasRecentParentUpdate(ConcurrentDictionary<Guid, DateTime> recentUpdates, Guid id, DateTime now)
    {
        return id != Guid.Empty &&
            recentUpdates.TryGetValue(id, out DateTime updatedAt) &&
            now - updatedAt <= ChildUpdateSuppressionWindow;
    }

    private static void RemoveExpiredParentUpdates(ConcurrentDictionary<Guid, DateTime> recentUpdates, DateTime now)
    {
        foreach (var (id, updatedAt) in recentUpdates)
        {
            if (now - updatedAt > ChildUpdateSuppressionWindow)
            {
                recentUpdates.TryRemove(id, out _);
            }
        }
    }

    private bool ShouldQueueItem(BaseItem item, NotificationFilter.NotificationType notificationType)
    {
        if (notificationType != NotificationFilter.NotificationType.ItemUpdated)
        {
            return true;
        }

        DateTime now = DateTime.UtcNow;
        RemoveExpiredParentUpdates(_recentUpdatedSeasons, now);
        RemoveExpiredParentUpdates(_recentUpdatedSeries, now);

        switch (item)
        {
            case Series series:
                _recentUpdatedSeries.AddOrUpdate(series.Id, now, (_, _) => now);
                return true;

            case Season season:
                _recentUpdatedSeasons.AddOrUpdate(season.Id, now, (_, _) => now);
                Guid seriesId = GetSeasonSeriesId(season);
                if (seriesId != Guid.Empty)
                {
                    _recentUpdatedSeries.AddOrUpdate(seriesId, now, (_, _) => now);
                }

                return true;

            case Episode episode:
                return !HasRecentParentUpdate(_recentUpdatedSeasons, GetEpisodeSeasonId(episode), now) &&
                    !HasRecentParentUpdate(_recentUpdatedSeries, GetEpisodeSeriesId(episode), now);

            default:
                return true;
        }
    }

    private void QueueItem(ItemChangeEventArgs itemChangeEventArgs, NotificationFilter.NotificationType notificationType)
    {
        // Never notify on virtual items.
        if (itemChangeEventArgs.Item.IsVirtualItem)
        {
            return;
        }

        // Only notify on books, movies, series, seasons, episodes, albums and audio.
        if (itemChangeEventArgs.Item.GetType() == typeof(MediaBrowser.Controller.Entities.Movies.Movie) ||
        itemChangeEventArgs.Item.GetType() == typeof(MediaBrowser.Controller.Entities.TV.Series) ||
        itemChangeEventArgs.Item.GetType() == typeof(MediaBrowser.Controller.Entities.TV.Season) ||
        itemChangeEventArgs.Item.GetType() == typeof(MediaBrowser.Controller.Entities.TV.Episode) ||
        itemChangeEventArgs.Item.GetType() == typeof(MediaBrowser.Controller.Entities.Audio.MusicAlbum) ||
        itemChangeEventArgs.Item.GetType() == typeof(MediaBrowser.Controller.Entities.Audio.Audio) ||
        itemChangeEventArgs.Item.GetType() == typeof(MediaBrowser.Controller.Entities.Book) ||
        itemChangeEventArgs.Item.GetType() == typeof(MediaBrowser.Controller.Entities.AudioBook))
        {
            if (!ShouldQueueItem(itemChangeEventArgs.Item, notificationType))
            {
                return;
            }

            _itemAddedManager.AddItem(itemChangeEventArgs.Item, notificationType);
        }
    }

    private void ItemAddedHandler(object? sender, ItemChangeEventArgs itemChangeEventArgs)
    {
        QueueItem(itemChangeEventArgs, NotificationFilter.NotificationType.ItemAdded);
    }

    private void ItemUpdatedHandler(object? sender, ItemChangeEventArgs itemChangeEventArgs)
    {
        QueueItem(itemChangeEventArgs, NotificationFilter.NotificationType.ItemUpdated);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _libraryManager.ItemAdded += ItemAddedHandler;
        _libraryManager.ItemUpdated += ItemUpdatedHandler;
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _libraryManager.ItemAdded -= ItemAddedHandler;
        _libraryManager.ItemUpdated -= ItemUpdatedHandler;
        return Task.CompletedTask;
    }
}
