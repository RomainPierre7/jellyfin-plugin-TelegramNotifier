using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using MediaBrowser.Controller;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Entities.Audio;
using MediaBrowser.Controller.Entities.TV;
using MediaBrowser.Controller.Library;
using MediaBrowser.Model.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.TelegramNotifier.Notifiers.ItemAddedNotifier;

public class ItemAddedManager : IItemAddedManager
{
    private const int MaxRetries = 10;
    private readonly ILogger<ItemAddedManager> _logger;
    private readonly ILibraryManager _libraryManager;
    private readonly IServerApplicationHost _applicationHost;
    private readonly ItemNotificationStore _itemNotificationStore;
    private readonly ConcurrentDictionary<Guid, QueuedItemContainer> _itemProcessQueue;
    private readonly ConcurrentDictionary<Guid, byte> _itemUpdateQueue;

    public ItemAddedManager(
        ILogger<ItemAddedManager> logger,
        ILibraryManager libraryManager,
        IServerApplicationHost applicationHost,
        ItemNotificationStore itemNotificationStore)
    {
        _logger = logger;
        _libraryManager = libraryManager;
        _applicationHost = applicationHost;
        _itemNotificationStore = itemNotificationStore;
        _itemProcessQueue = new ConcurrentDictionary<Guid, QueuedItemContainer>();
        _itemUpdateQueue = new ConcurrentDictionary<Guid, byte>();
    }

    public async Task ProcessItemsAsync()
    {
        _logger.LogDebug("ProcessItemsAsync");
        // Attempt to process all items in queue.
        var currentItems = _itemProcessQueue.ToArray();
        var updatedItems = _itemUpdateQueue.ToArray();
        if (currentItems.Length != 0 || updatedItems.Length != 0)
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
                        continue;
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

                    string subtype = GetSubtype(item);
                    string serverUrl = Plugin.Instance?.Configuration.ServerUrl ?? "localhost:8096";
                    string path = "http://" + serverUrl + "/Items/" + item.Id + "/Images/Primary";

                    var records = await notificationFilter.Filter(
                        NotificationFilter.NotificationType.ItemAdded,
                        item,
                        imagePath: path,
                        subtype: subtype,
                        trackedItemId: item.Id).ConfigureAwait(false);
                    await _itemNotificationStore.AddAsync(records).ConfigureAwait(false);

                    // Remove item from queue.
                    _itemProcessQueue.TryRemove(key, out _);
                }

                foreach (var updatedItem in updatedItems)
                {
                    Guid key = updatedItem.Key;
                    _itemUpdateQueue.TryRemove(key, out _);
                    try
                    {
                        var records = await _itemNotificationStore.GetAsync(key).ConfigureAwait(false);
                        if (records.Count == 0)
                        {
                            continue;
                        }

                        BaseItem? item = _libraryManager.GetItemById(key);
                        if (item is not null)
                        {
                            await notificationFilter.UpdateItemAddedNotifications(item, GetSubtype(item), records).ConfigureAwait(false);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Could not update Telegram notifications for item {ItemId}", key);
                    }
                }

                await _itemNotificationStore.PurgeExpiredAsync().ConfigureAwait(false);
            }
        }
        else
        {
            _logger.LogInformation("No items to process in the queue");
        }
    }

    public void AddItem(BaseItem item)
    {
        LibraryOptions options = _libraryManager.GetLibraryOptions(item);
        if (options.Enabled)
        {
            _itemProcessQueue.TryAdd(item.Id, new QueuedItemContainer(item.Id));
            _logger.LogDebug("Queued {ItemName} for notification", item.Name);
        }
        else
        {
            _logger.LogDebug("Not queueing {ItemName} for notification because the it is a disabled library", item.Name);
        }

    }

    public void UpdateItem(BaseItem item)
    {
        _itemUpdateQueue.TryAdd(item.Id, 0);
    }

    private static string GetSubtype(BaseItem item)
    {
        return item switch
        {
            Series => "ItemAddedSeries",
            Season => "ItemAddedSeasons",
            Episode => "ItemAddedEpisodes",
            MusicAlbum => "ItemAddedAlbums",
            Audio => "ItemAddedSongs",
            Book => "ItemAddedBooks",
            _ => "ItemAddedMovies"
        };
    }
}
