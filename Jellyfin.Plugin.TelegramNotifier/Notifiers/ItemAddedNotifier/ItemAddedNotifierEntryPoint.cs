using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Controller.Library;
using Microsoft.Extensions.Hosting;

namespace Jellyfin.Plugin.TelegramNotifier.Notifiers.ItemAddedNotifier;

public class ItemAddedNotifierEntryPoint : IHostedService
{
    private readonly IItemAddedManager _itemAddedManager;
    private readonly ILibraryManager _libraryManager;

    public ItemAddedNotifierEntryPoint(
        IItemAddedManager itemAddedManager,
        ILibraryManager libraryManager)
    {
        _itemAddedManager = itemAddedManager;
        _libraryManager = libraryManager;
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
