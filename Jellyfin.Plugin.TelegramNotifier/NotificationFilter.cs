using System;
using System.Threading.Tasks;

namespace Jellyfin.Plugin.TelegramNotifier
{
    public class NotificationFilter
    {
        private readonly Sender _sender;

        public NotificationFilter(Sender sender)
        {
            _sender = sender;
        }

        public async Task Filter(string notificationType, string message)
        {
            if (Plugin.Config.EnablePlugin == false)
            {
                return;
            }

            string botToken = Plugin.Config.BotToken;
            string chatId = Plugin.Config.ChatId;
            await _sender.SendMessage(notificationType, message, botToken, chatId).ConfigureAwait(false);
        }
    }
}