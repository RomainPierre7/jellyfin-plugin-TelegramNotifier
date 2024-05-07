using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Library;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.TelegramNotifier.Notifiers.ItemAddedNotifier;

public class ItemAddedManager : IItemAddedManager
{
    private const int MaxRetries = 10;
    private readonly ILogger<ItemAddedManager> _logger;
    private readonly ILibraryManager _libraryManager;
    private readonly NotificationFilter _notificationFilter;
    private readonly ConcurrentDictionary<Guid, QueuedItemContainer> _itemProcessQueue;

    public ItemAddedManager(
        ILogger<ItemAddedManager> logger,
        ILibraryManager libraryManager,
        NotificationFilter notificationFilter)
    {
        _logger = logger;
        _libraryManager = libraryManager;
        _notificationFilter = notificationFilter;
        _itemProcessQueue = new ConcurrentDictionary<Guid, QueuedItemContainer>();
    }

    public async Task ProcessItemsAsync()
    {
        _logger.LogDebug("ProcessItemsAsync");
        // Attempt to process all items in queue.
        var currentItems = _itemProcessQueue.ToArray();
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

            // Send notification to each configured destination.
            string message = $"🎬 {item.Name} added to library";

            await _notificationFilter.Filter(NotificationFilter.NotificationType.ItemAdded, message).ConfigureAwait(false);

            // Remove item from queue.
            _itemProcessQueue.TryRemove(key, out _);
        }
    }

    public void AddItem(BaseItem item)
    {
        _itemProcessQueue.TryAdd(item.Id, new QueuedItemContainer(item.Id));
        _logger.LogDebug("Queued {ItemName} for notification", item.Name);
    }
}
