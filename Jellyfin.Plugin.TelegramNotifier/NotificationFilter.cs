using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jellyfin.Plugin.TelegramNotifier.Configuration;

namespace Jellyfin.Plugin.TelegramNotifier
{
    public class NotificationFilter
    {
        private readonly Sender _sender;

        public NotificationFilter(Sender sender)
        {
            _sender = sender;
        }

        public enum NotificationType
        {
            ItemAdded,
            Generic,
            PlaybackStart,
            PlaybackProgress,
            PlaybackStop,
            SubtitleDownloadFailure,
            AuthenticationFailure,
            AuthenticationSuccess,
            SessionStart,
            PendingRestart,
            TaskCompleted,
            PluginInstallationCancelled,
            PluginInstallationFailed,
            PluginInstalled,
            PluginInstalling,
            PluginUninstalled,
            PluginUpdated,
            UserCreated,
            UserDeleted,
            UserLockedOut,
            UserPasswordChanged,
            UserUpdated,
            UserDataSaved
        }

        private bool GetPropertyValue(UserConfiguration user, string propertyName)
        {
            var property = user.GetType().GetProperty(propertyName);
            if (property != null)
            {
                var value = property.GetValue(user);
                if (value != null)
                {
                    return (bool)value;
                }
                else
                {
                    throw new ArgumentException($"The property {propertyName} is null.");
                }
            }
            else
            {
                throw new ArgumentException($"The property {propertyName} does not exist.");
            }
        }

        public async Task Filter(NotificationType type, string message)
        {
            if (!Plugin.Config.EnablePlugin)
            {
                return;
            }

            UserConfiguration[] users = Plugin.Config.UserConfigurations;
            var tasks = new List<Task>();

            foreach (UserConfiguration user in users)
            {
                if (user.EnableUser == false)
                {
                    continue;
                }

                bool isNotificationTypeEnabled = GetPropertyValue(user, type.ToString());
                if (!isNotificationTypeEnabled)
                {
                    continue;
                }

                string botToken = user.BotToken ?? string.Empty;
                string chatId = user.ChatId ?? string.Empty;

                try
                {
                    Task task = _sender.SendMessage(type.ToString(), message, botToken, chatId);
                    tasks.Add(task);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while sending a message: {ex.Message}");
                }
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);
        }
    }
}