using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Controller.Library;
using Microsoft.Extensions.Hosting;

namespace Jellyfin.Plugin.TelegramNotifier.Notifiers.ItemDeletedNotifier;

public class ItemDeletedNotifierEntryPoint : IHostedService
{
    private readonly IItemDeletedManager _itemDeletedManager;
    private readonly ILibraryManager _libraryManager;

    public ItemDeletedNotifierEntryPoint(
        IItemDeletedManager itemDeletedManager,
        ILibraryManager libraryManager)
    {
        _itemDeletedManager = itemDeletedManager;
        _libraryManager = libraryManager;
    }

    private void ItemDeletedHandler(object? sender, ItemChangeEventArgs itemChangeEventArgs)
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
            _itemDeletedManager.AddItem(itemChangeEventArgs.Item);
        }
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _libraryManager.ItemRemoved += ItemDeletedHandler;
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _libraryManager.ItemRemoved -= ItemDeletedHandler;
        return Task.CompletedTask;
    }
}
