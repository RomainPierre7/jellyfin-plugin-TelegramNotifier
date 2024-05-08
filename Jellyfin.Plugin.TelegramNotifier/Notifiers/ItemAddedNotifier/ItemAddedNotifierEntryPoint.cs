using System;
using System.Threading.Tasks;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Library;
using MediaBrowser.Controller.Plugins;

namespace Jellyfin.Plugin.TelegramNotifier.Notifiers.ItemAddedNotifier;

public class ItemAddedNotifierEntryPoint : IServerEntryPoint
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

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public Task RunAsync()
    {
        _libraryManager.ItemAdded += ItemAddedHandler;
        return Task.CompletedTask;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _libraryManager.ItemAdded -= ItemAddedHandler;
        }
    }

    private void ItemAddedHandler(object? sender, ItemChangeEventArgs itemChangeEventArgs)
    {
        // Never notify on virtual items.
        if (itemChangeEventArgs.Item.IsVirtualItem)
        {
            return;
        }

        // Only notify on movies, series, seasons, episodes, and audio.
        if (itemChangeEventArgs.Item.GetType() == typeof(MediaBrowser.Controller.Entities.Movies.Movie) ||
            itemChangeEventArgs.Item.GetType() == typeof(MediaBrowser.Controller.Entities.TV.Series) ||
            itemChangeEventArgs.Item.GetType() == typeof(MediaBrowser.Controller.Entities.TV.Season) ||
            itemChangeEventArgs.Item.GetType() == typeof(MediaBrowser.Controller.Entities.TV.Episode) ||
            itemChangeEventArgs.Item.GetType() == typeof(MediaBrowser.Controller.Entities.Audio.Audio))
        {
            _itemAddedManager.AddItem(itemChangeEventArgs.Item);
        }
    }
}
