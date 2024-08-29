using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Threading.Tasks;
using MediaBrowser.Controller;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Entities.Audio;
using MediaBrowser.Controller.Entities.TV;
using MediaBrowser.Controller.Library;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.TelegramNotifier.Notifiers.ItemAddedNotifier;

public class ItemAddedManager : IItemAddedManager
{
    private const int MaxRetries = 10;
    private readonly ILogger<ItemAddedManager> _logger;
    private readonly ILibraryManager _libraryManager;
    private readonly IServerApplicationHost _applicationHost;
    private readonly ConcurrentDictionary<Guid, QueuedItemContainer> _itemProcessQueue;

    public ItemAddedManager(
        ILogger<ItemAddedManager> logger,
        ILibraryManager libraryManager,
        IServerApplicationHost applicationHost)
    {
        _logger = logger;
        _libraryManager = libraryManager;
        _applicationHost = applicationHost;
        _itemProcessQueue = new ConcurrentDictionary<Guid, QueuedItemContainer>();
    }

    public async Task ProcessItemsAsync()
    {
        _logger.LogDebug("ProcessItemsAsync");
        // Attempt to process all items in queue.
        var currentItems = _itemProcessQueue.ToArray();
        if (currentItems.Length != 0)
        {
            var scope = _applicationHost.ServiceProvider!.CreateAsyncScope();
            var notificationFilter = scope.ServiceProvider.GetRequiredService<NotificationFilter>();
            await using (scope.ConfigureAwait(false))
            {
                foreach (var (key, container) in currentItems)
                {
                    var item = _libraryManager.GetItemById(key);
                    if (item is null)
                    {
                        // Remove item from queue.
                        _itemProcessQueue.TryRemove(key, out _);
                        return;
                    }

                    _logger.LogDebug("Item {ItemName}", item.Name);

                    // Metadata not refreshed yet and under retry limit.
                    if (item.ProviderIds.Keys.Count == 0 && container.RetryCount < MaxRetries)
                    {
                        _logger.LogDebug("Requeue {ItemName}, no provider ids", item.Name);
                        container.RetryCount++;
                        _itemProcessQueue.AddOrUpdate(key, container, (_, _) => container);
                        continue;
                    }

                    _logger.LogDebug("Notifying for {ItemName}", item.Name);

                    // Send notification.
                    string message = $"🎬 {item.Name} ({item.ProductionYear})\n" +
                                     $"      added to library";

                    string subtype = "ItemAddedMovies";
                    bool addImage = true;

                    switch (item)
                    {
                        case Series serie:
                            message = $"📺 [Serie] {serie.Name} ({item.ProductionYear}) added to library";
                            subtype = "ItemAddedSeries";
                            break;

                        case Season season:
                            string seasonNumber = season.IndexNumber.HasValue ? season.IndexNumber.Value.ToString("00", CultureInfo.InvariantCulture) : "00";

                            message = $"📺 {season.Series.Name} ({item.ProductionYear})\n" +
                                      $"      Season {seasonNumber} added to library";
                            subtype = "ItemAddedSeasons";
                            break;

                        case Episode episode:
                            addImage = false;
                            string eSeasonNumber = episode.Season.IndexNumber.HasValue ? episode.Season.IndexNumber.Value.ToString("00", CultureInfo.InvariantCulture) : "00";
                            string episodeNumber = episode.IndexNumber.HasValue ? episode.IndexNumber.Value.ToString("00", CultureInfo.InvariantCulture) : "00";

                            message = $"📺 {episode.Series.Name} ({item.ProductionYear})\n" +
                                      $"      S{eSeasonNumber} - E{episodeNumber}\n" +
                                      $"      '{item.Name}' added to library";
                            subtype = "ItemAddedEpisodes";
                            break;

                        case MusicAlbum album:
                            message = $"🎵 [Album] {album.Name} ({item.ProductionYear}) added to library";
                            subtype = "ItemAddedAlbums";
                            break;

                        case Audio audio:
                            message = $"🎵 [Audio] {audio.Name} ({item.ProductionYear}) added to library";
                            subtype = "ItemAddedAudios";
                            break;
                    }

                    if (item.PrimaryImagePath is not null && addImage)
                    {
                        string serverUrl = Plugin.Instance?.Configuration.ServerUrl ?? "localhost:8096";
                        string path = "http://" + serverUrl + "/Items/" + item.Id + "/Images/Primary";

                        await notificationFilter.Filter(NotificationFilter.NotificationType.ItemAdded, message, imagePath: path, subtype: subtype).ConfigureAwait(false);
                    }
                    else
                    {
                        await notificationFilter.Filter(NotificationFilter.NotificationType.ItemAdded, message, subtype: subtype).ConfigureAwait(false);
                    }

                    // Remove item from queue.
                    _itemProcessQueue.TryRemove(key, out _);
                }
            }
        }
    }

    public void AddItem(BaseItem item)
    {
        _itemProcessQueue.TryAdd(item.Id, new QueuedItemContainer(item.Id));
        _logger.LogDebug("Queued {ItemName} for notification", item.Name);
    }
}
