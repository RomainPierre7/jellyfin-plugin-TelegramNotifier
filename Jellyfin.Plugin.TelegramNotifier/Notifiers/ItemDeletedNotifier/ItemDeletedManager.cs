using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using MediaBrowser.Controller;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Entities.Audio;
using MediaBrowser.Controller.Entities.TV;
using MediaBrowser.Controller.Library;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.TelegramNotifier.Notifiers.ItemDeletedNotifier;

public class ItemDeletedManager : IItemDeletedManager
{
    private readonly ILogger<ItemDeletedManager> _logger;
    private readonly ILibraryManager _libraryManager;
    private readonly IServerApplicationHost _applicationHost;
    private readonly ConcurrentDictionary<Guid, BaseItem> _itemProcessQueue;

    public ItemDeletedManager(
        ILogger<ItemDeletedManager> logger,
        ILibraryManager libraryManager,
        IServerApplicationHost applicationHost)
    {
        _logger = logger;
        _libraryManager = libraryManager;
        _applicationHost = applicationHost;
        _itemProcessQueue = new ConcurrentDictionary<Guid, BaseItem>();
    }

    public async Task ProcessItemsAsync()
    {
        _logger.LogDebug("ProcessItemsAsync");
        // Attempt to process all items in queue.
        if (!_itemProcessQueue.IsEmpty)
        {
            var scope = _applicationHost.ServiceProvider!.CreateAsyncScope();
            await using (scope.ConfigureAwait(false))
            {
                var notificationFilter = scope.ServiceProvider.GetRequiredService<NotificationFilter>();
                foreach (var (key, item) in _itemProcessQueue)
                {
                    if (item != null)
                    {
                        _logger.LogDebug("Item {ItemName}", item.Name);

                        // Skip notification if item type is Studio
                        if (item.GetType().Name == "Studio")
                        {
                            _logger.LogDebug("Skipping notification for item type Studio");
                            _itemProcessQueue.TryRemove(key, out _);
                            continue;
                        }

                        _logger.LogDebug("Notifying for {ItemName}", item.Name);

                        string subtype = "ItemDeletedMovies";
                        bool addImage = true;

                        dynamic eventArgs = item;

                        switch (item)
                        {
                            case Series serie:
                                subtype = "ItemDeletedSeries";
                                eventArgs = serie;
                                break;

                            case Season season:
                                subtype = "ItemDeletedSeasons";
                                eventArgs = season;
                                break;

                            case Episode episode:
                                addImage = false;
                                subtype = "ItemDeletedEpisodes";
                                eventArgs = episode;
                                break;

                            case MusicAlbum album:
                                subtype = "ItemDeletedAlbums";
                                eventArgs = album;
                                break;

                            case Audio audio:
                                subtype = "ItemDeletedAudios";
                                eventArgs = audio;
                                break;

                            case Book book:
                                subtype = "ItemDeletedBooks";
                                eventArgs = book;
                                break;
                        }

                        await notificationFilter.Filter(NotificationFilter.NotificationType.ItemDeleted, eventArgs, subtype: subtype).ConfigureAwait(false);

                        // Remove item from queue.
                        _itemProcessQueue.TryRemove(key, out _);
                    }
                }
            }
        }
    }

    public void AddItem(BaseItem item)
    {
        _itemProcessQueue.TryAdd(item.Id, item);
        _logger.LogDebug("Queued {ItemName} for notification", item.Name);
    }
}
