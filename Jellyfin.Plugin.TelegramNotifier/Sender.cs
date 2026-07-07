using System;
using System.Collections.Generic;
using System.Net;
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
            _logger = Plugin.Logger;
            var handler = new HttpClientHandler();

            if (Plugin.Instance.Configuration.ProxyEnabled && !string.IsNullOrEmpty(Plugin.Instance.Configuration.ProxyHost))
            {
                var proxy = new WebProxy(
                    Plugin.Instance.Configuration.ProxyHost,
                    Plugin.Instance.Configuration.ProxyPort);

                if (!string.IsNullOrEmpty(Plugin.Instance.Configuration.ProxyUsername))
                {
                    proxy.Credentials = new NetworkCredential(
                        Plugin.Instance.Configuration.ProxyUsername,
                        Plugin.Instance.Configuration.ProxyPassword);

                    _logger.LogInformation(
                        "TelegramNotifier: Using HTTP proxy {ProxyHost}:{ProxyPort} with authentication.",
                        Plugin.Instance.Configuration.ProxyHost,
                        Plugin.Instance.Configuration.ProxyPort);
                }
                else
                {
                    _logger.LogInformation(
                        "TelegramNotifier: Using HTTP proxy {ProxyHost}:{ProxyPort} without authentication.",
                        Plugin.Instance.Configuration.ProxyHost,
                        Plugin.Instance.Configuration.ProxyPort);
                }

                handler.Proxy = proxy;
                handler.UseProxy = true;
            }

            _httpClient = new HttpClient(handler);
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

        public async Task<bool> SendMessage(string notificationType, string message, string botToken, string chatId, bool isSilentNotification, string threadId)
        {
            try
            {
                string url = $"https://api.telegram.org/bot{botToken}/sendMessage";

                var parameters = new Dictionary<string, string>
                {
                    { "chat_id", chatId },
                    { "text", message },
                    { "parse_mode", "HTML" }
                };

                if (isSilentNotification)
                {
                    parameters.Add("disable_notification", "true");
                }

                if (!string.IsNullOrEmpty(threadId))
                {
                    parameters.Add("message_thread_id", threadId);
                }

                var content = new FormUrlEncodedContent(parameters);

                HttpResponseMessage response = await _httpClient.PostAsync(url, content).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();

                _logger.LogInformation("({NotificationType}): Message sent successfully.", notificationType);
                return true;
            }
            catch (HttpRequestException)
            {
                _logger.LogError("({NotificationType}): Message could not be sent, please check your configuration (botToken: {BotToken}, chatId: {ChatId}, threadId: {ThreadId}), your template, your internet connection or the Telegram API.", notificationType, botToken, chatId, threadId);
                return false;
            }
        }

        public async Task<bool> SendMessageWithPhoto(string notificationType, string message, string imageUrl, string botToken, string chatId, bool isSilentNotification, string threadId)
        {
            try
            {
                string url = $"https://api.telegram.org/bot{botToken}/sendPhoto";

                MultipartFormDataContent formData = new MultipartFormDataContent();

                formData.Add(new StringContent(chatId), "chat_id");
                formData.Add(new StringContent(message), "caption");
                formData.Add(new StringContent(isSilentNotification ? "true" : "false"), "disable_notification");
                formData.Add(new StringContent("HTML"), "parse_mode");

                if (!string.IsNullOrEmpty(threadId))
                {
                    formData.Add(new StringContent(threadId), "message_thread_id");
                }

                _logger.LogInformation("Reaching for image: {Url}", imageUrl);
                HttpResponseMessage imageResponse = await _httpClient.GetAsync(imageUrl).ConfigureAwait(false);
                imageResponse.EnsureSuccessStatusCode();
                byte[] imageBytes = await imageResponse.Content.ReadAsByteArrayAsync().ConfigureAwait(false);

                ByteArrayContent imageContent = new ByteArrayContent(imageBytes);
                formData.Add(imageContent, "photo", "image.png");

                HttpResponseMessage response = await _httpClient.PostAsync(url, formData).ConfigureAwait(false);
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
