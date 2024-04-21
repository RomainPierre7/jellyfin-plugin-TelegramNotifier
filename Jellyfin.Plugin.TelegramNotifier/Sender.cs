using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.TelegramNotifier
{
    public class Sender : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<Plugin> _logger;

        public Sender()
        {
            _httpClient = new HttpClient();
            _logger = Plugin.Logger;
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

        public async Task<bool> SendMessage(string notificationType, string message, string botToken, string chatId)
        {
            try
            {
                string url = $"https://api.telegram.org/bot{botToken}/sendMessage?chat_id={chatId}&text={message}";
                HttpResponseMessage response = await _httpClient.GetAsync(url).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();

                _logger.LogInformation("({NotificationType}): Message sent successfully.", notificationType);
                return true;
            }
            catch (HttpRequestException)
            {
                _logger.LogError("({NotificationType}): Message could not be sent, please check your configuration.", notificationType);
                return false;
            }
        }
    }
}