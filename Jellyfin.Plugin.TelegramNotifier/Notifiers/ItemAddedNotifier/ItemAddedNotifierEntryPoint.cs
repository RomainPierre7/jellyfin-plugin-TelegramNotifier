using System;
using System.Threading.Tasks;
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

        _itemAddedManager.AddItem(itemChangeEventArgs.Item);
    }
}
