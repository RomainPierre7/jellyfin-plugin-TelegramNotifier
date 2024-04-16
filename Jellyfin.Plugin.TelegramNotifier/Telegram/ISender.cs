using System.Threading.Tasks;

namespace Jellyfin.Plugin.TelegramNotifier.Telegram
{
    public interface ISender
    {
        Task SendMessage(string message);
    }
}
