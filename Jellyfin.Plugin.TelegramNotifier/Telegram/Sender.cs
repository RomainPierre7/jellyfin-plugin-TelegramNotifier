using System;
using System.Net.Http;
using System.Threading.Tasks;
using Jellyfin.Plugin.TelegramNotifier.Configuration;

namespace Jellyfin.Plugin.TelegramNotifier.Telegram
{
    public class Sender : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly string botToken;
        private readonly string chatId;

        public Sender()
        {
            _httpClient = new HttpClient();

            PluginConfiguration configuration = Plugin.Instance!.Configuration;
            botToken = configuration.BotToken;
            chatId = configuration.ChatID;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _httpClient.Dispose();
            }
        }

        public async Task<bool> SendMessage(string message)
        {
            try
            {
                string url = $"https://api.telegram.org/bot{botToken}/sendMessage?chat_id={chatId}&text={message}";
                HttpResponseMessage response = await _httpClient.GetAsync(url).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (HttpRequestException)
            {
                return false;
            }
        }
    }
}