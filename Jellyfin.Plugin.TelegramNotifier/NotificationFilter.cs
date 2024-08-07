using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jellyfin.Plugin.TelegramNotifier.Configuration;

namespace Jellyfin.Plugin.TelegramNotifier
{
    public class NotificationFilter
    {
        private readonly Sender _sender;
        private readonly NotificationFormatter _formatter;

        public NotificationFilter(Sender sender, NotificationFormatter formatter)
        {
            _sender = sender;
            _formatter = formatter;
        }

        public enum NotificationType
        {
            ItemAdded,
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

        public async Task Filter(NotificationType type, string message, string userId = "", string imagePath = "", string subtype = "", string overview = "")
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

                if (!string.IsNullOrEmpty(subtype))
                {
                    bool isSubTypeEnabled = GetPropertyValue(user, subtype);
                    if (!isSubTypeEnabled)
                    {
                        continue;
                    }
                }

                if (user.DoNotMentionOwnActivities == true && user.UserId is not null)
                {
                    string currentUserid = user.UserId.Replace("-", string.Empty, StringComparison.OrdinalIgnoreCase);
                    string notifUserId = userId.Replace("-", string.Empty, StringComparison.OrdinalIgnoreCase);
                    if (currentUserid == notifUserId)
                    {
                        continue;
                    }
                }

                string botToken = user.BotToken ?? string.Empty;
                string chatId = user.ChatId ?? string.Empty;
                bool isSilentNotification = user.SilentNotification ?? false;
                bool isEnableItemAddedImage = user.EnableItemAddedImage ?? false;

                // Format the message with prefix and suffix
                string formattedMessage = _formatter.FormatMessage(type, message, user, subtype, overview);

                try
                {
                    if (string.IsNullOrEmpty(imagePath) || !isEnableItemAddedImage)
                    {
                        Task task = _sender.SendMessage(type.ToString(), formattedMessage, botToken, chatId, isSilentNotification);
                        tasks.Add(task);
                    }
                    else
                    {
                        Task task = _sender.SendMessageWithPhoto(type.ToString(), formattedMessage, imagePath, botToken, chatId, isSilentNotification);
                        tasks.Add(task);
                    }
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