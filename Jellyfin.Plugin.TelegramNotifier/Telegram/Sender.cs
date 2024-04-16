using System.Threading.Tasks;
using Jellyfin.Plugin.TelegramNotifier.Configuration;
using Telegram.Bot;

namespace Jellyfin.Plugin.TelegramNotifier.Telegram
{
    public class Sender : ISender
    {
        private readonly string botToken;
        private readonly string chatId;

        public Sender()
        {
            PluginConfiguration configuration = Plugin.Instance!.Configuration;
            botToken = configuration.BotToken;
            chatId = configuration.ChatID;
        }

        public async Task SendMessage(string message)
        {
            var bot = new TelegramBotClient(botToken);
            await bot.SendTextMessageAsync(chatId, message).ConfigureAwait(false);
        }
    }
}
