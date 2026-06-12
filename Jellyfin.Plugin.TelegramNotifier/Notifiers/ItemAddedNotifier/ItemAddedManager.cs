using System;
using System.Collections.Concurrent;
using System.Linq;
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
    private static readonly TimeSpan AddedUpdateSuppressionWindow = TimeSpan.FromMinutes(5);
    private readonly ILogger<ItemAddedManager> _logger;
    private readonly ILibraryManager _libraryManager;
    private readonly IServerApplicationHost _applicationHost;
    private readonly ConcurrentDictionary<string, QueuedItemContainer> _itemProcessQueue;
    private readonly ConcurrentDictionary<Guid, DateTime> _recentSuccessfulAdds;

    public ItemAddedManager(
        ILogger<ItemAddedManager> logger,
        ILibraryManager libraryManager,
        IServerApplicationHost applicationHost)
    {
        _logger = logger;
        _libraryManager = libraryManager;
        _applicationHost = applicationHost;
        _itemProcessQueue = new ConcurrentDictionary<string, QueuedItemContainer>();
        _recentSuccessfulAdds = new ConcurrentDictionary<Guid, DateTime>();
    }

    private static bool IsItemUpdated(QueuedItemContainer container)
    {
        return container.NotificationType == NotificationFilter.NotificationType.ItemUpdated;
    }

    private void RemoveExpiredSuccessfulAdds(DateTime now)
    {
        foreach (var (id, sentAt) in _recentSuccessfulAdds)
        {
            if (now - sentAt > AddedUpdateSuppressionWindow)
            {
                _recentSuccessfulAdds.TryRemove(id, out _);
            }
        }
    }

    private bool HasRecentSuccessfulAdd(Guid itemId, DateTime now)
    {
        return _recentSuccessfulAdds.TryGetValue(itemId, out DateTime sentAt) &&
            now - sentAt <= AddedUpdateSuppressionWindow;
    }

    private void RecordSuccessfulAdd(Guid itemId)
    {
        DateTime now = DateTime.UtcNow;
        _recentSuccessfulAdds.AddOrUpdate(itemId, now, (_, _) => now);
    }

    public async Task ProcessItemsAsync()
    {
        _logger.LogDebug("ProcessItemsAsync");
        // Attempt to process all items in queue.
        var currentItems = _itemProcessQueue.ToArray()
            .OrderBy(item => IsItemUpdated(item.Value))
            .ToArray();
        if (currentItems.Length != 0)
        {
            var scope = _applicationHost.ServiceProvider!.CreateAsyncScope();
            var notificationFilter = scope.ServiceProvider.GetRequiredService<NotificationFilter>();
            await using (scope.ConfigureAwait(false))
            {
                foreach (var (key, container) in currentItems)
                {
                    var item = _libraryManager.GetItemById(container.ItemId);

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

                    DateTime now = DateTime.UtcNow;
                    RemoveExpiredSuccessfulAdds(now);
                    if (container.NotificationType == NotificationFilter.NotificationType.ItemUpdated && HasRecentSuccessfulAdd(item.Id, now))
                    {
                        _logger.LogInformation("Skipping ItemUpdated for {ItemName} because ItemAdded was sent successfully less than five minutes ago", item.Name);
                        _itemProcessQueue.TryRemove(key, out _);
                        continue;
                    }

                    string notificationTypeName = container.NotificationType.ToString();
                    string subtype = notificationTypeName + "Movies";
                    bool addImage = true;

                    dynamic eventArgs = item;

                    switch (item)
                    {
                        case Series serie:
                            subtype = notificationTypeName + "Series";
                            eventArgs = serie;
                            break;

                        case Season season:
                            subtype = notificationTypeName + "Seasons";
                            eventArgs = season;
                            break;

                        case Episode episode:
                            subtype = notificationTypeName + "Episodes";
                            eventArgs = episode;
                            break;

                        case MusicAlbum album:
                            subtype = notificationTypeName + "Albums";
                            eventArgs = album;
                            break;

                        case Audio audio:
                            subtype = notificationTypeName + "Songs";
                            eventArgs = audio;
                            break;

                        case Book book:
                            subtype = notificationTypeName + "Books";
                            eventArgs = book;
                            break;
                    }

                    if (addImage)
                    {
                        string serverUrl = Plugin.Instance?.Configuration.ServerUrl ?? "localhost:8096";
                        serverUrl = serverUrl.Trim().TrimEnd('/');
                        serverUrl = serverUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase) ? serverUrl : "http://" + serverUrl;
                        string path = serverUrl + "/Items/" + item.Id + "/Images/Primary";

                        bool sent = await notificationFilter.Filter(container.NotificationType, eventArgs, imagePath: path, subtype: subtype).ConfigureAwait(false);
                        if (sent && container.NotificationType == NotificationFilter.NotificationType.ItemAdded)
                        {
                            RecordSuccessfulAdd(item.Id);
                        }
                    }
                    else
                    {
                        bool sent = await notificationFilter.Filter(container.NotificationType, eventArgs, subtype: subtype).ConfigureAwait(false);
                        if (sent && container.NotificationType == NotificationFilter.NotificationType.ItemAdded)
                        {
                            RecordSuccessfulAdd(item.Id);
                        }
                    }

                    // Remove item from queue.
                    _itemProcessQueue.TryRemove(key, out _);
                }
            }
        }
        else
        {
            _logger.LogInformation("No items to process in the queue");
        }
    }

    public void AddItem(BaseItem item, NotificationFilter.NotificationType notificationType)
    {
        LibraryOptions options = _libraryManager.GetLibraryOptions(item);
        if (options.Enabled)
        {
            string key = item.Id + ":" + notificationType;
            _itemProcessQueue.TryAdd(key, new QueuedItemContainer(item.Id, notificationType));
            _logger.LogDebug("Queued {ItemName} for {NotificationType} notification", item.Name, notificationType);
        }
        else
        {
            _logger.LogDebug("Not queueing {ItemName} for notification because the it is a disabled library", item.Name);
        }
    }
}
