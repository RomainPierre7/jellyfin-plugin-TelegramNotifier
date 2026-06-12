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
    private readonly ConcurrentDictionary<string, QueuedItemContainer> _itemProcessQueue;

    public ItemAddedManager(
        ILogger<ItemAddedManager> logger,
        ILibraryManager libraryManager,
        IServerApplicationHost applicationHost)
    {
        _logger = logger;
        _libraryManager = libraryManager;
        _applicationHost = applicationHost;
        _itemProcessQueue = new ConcurrentDictionary<string, QueuedItemContainer>();
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

                        await notificationFilter.Filter(container.NotificationType, eventArgs, imagePath: path, subtype: subtype).ConfigureAwait(false);
                    }
                    else
                    {
                        await notificationFilter.Filter(container.NotificationType, eventArgs, subtype: subtype).ConfigureAwait(false);
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
