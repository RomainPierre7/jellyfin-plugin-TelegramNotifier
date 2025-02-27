using System.Threading.Tasks;
using MediaBrowser.Controller.Entities;

namespace Jellyfin.Plugin.TelegramNotifier.Notifiers.ItemDeletedNotifier;

public interface IItemDeletedManager
{
    public Task ProcessItemsAsync();

    public void AddItem(BaseItem item);
}
