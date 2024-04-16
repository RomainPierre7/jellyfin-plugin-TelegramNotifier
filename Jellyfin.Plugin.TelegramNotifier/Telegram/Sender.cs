using System;
using System.Net.Http;
using System.Threading.Tasks;
using Jellyfin.Plugin.TelegramNotifier;
using Jellyfin.Plugin.TelegramNotifier.Configuration;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.TelegramNotifier.Telegram
{
    public class Sender : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<Plugin> _logger;
        private readonly string botToken;
        private readonly string chatId;

        public Sender()
        {
            _httpClient = new HttpClient();
            _logger = Plugin.Logger;
            botToken = Plugin.Config.BotToken;
            chatId = Plugin.Config.ChatId;
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
                _logger.LogInformation("Message sent successfully");
                return true;
            }
            catch (HttpRequestException)
            {
                _logger.LogError("Message could not be sent, please check your configuration");
                return false;
            }
        }
    }
}