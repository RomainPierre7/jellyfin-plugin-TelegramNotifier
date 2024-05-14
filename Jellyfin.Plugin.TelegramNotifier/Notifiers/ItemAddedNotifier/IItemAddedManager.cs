using System.Threading.Tasks;
using MediaBrowser.Controller.Entities;

namespace Jellyfin.Plugin.TelegramNotifier.Notifiers.ItemAddedNotifier;

public interface IItemAddedManager
{
    public Task ProcessItemsAsync();

    public void AddItem(BaseItem item);
}
