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

        private (bool value, string message) GetPropertyValue(UserConfiguration user, string propertyName)
        {
            var property = user.GetType().GetProperty(propertyName);
            var property_message = user.GetType().GetProperty(propertyName + "StringMessage");
            if (property != null && property_message != null)
            {
                var value = property.GetValue(user);
                var message = property_message.GetValue(user);
                if (value != null && message != null)
                {
                    return ((bool)value, (string)message);
                }
                else
                {
                    throw new ArgumentException($"The property {propertyName} or {propertyName + "StringMessage"} is null.");
                }
            }
            else
            {
                throw new ArgumentException($"The property {propertyName} or {propertyName + "StringMessage"} does not exist.");
            }
        }

        public async Task Filter(NotificationType type, string message, string userId = "", string imagePath = "", string subtype = "")
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

                bool isNotificationTypeEnabled = GetPropertyValue(user, type.ToString()).value;
                if (!isNotificationTypeEnabled)
                {
                    continue;
                }

                if (!string.IsNullOrEmpty(subtype))
                {
                    bool isSubTypeEnabled = GetPropertyValue(user, subtype).value;
                    if (!isSubTypeEnabled)
                    {
                        continue;
                    }

                    message = GetPropertyValue(user, subtype).message;
                }
                else
                {
                    message = GetPropertyValue(user, type.ToString()).message;
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

                string botToken = user.BotToken;
                string chatId = user.ChatId;
                bool isSilentNotification = user.SilentNotification;
                string threadId = user.ThreadId;

                try
                {
                    if (string.IsNullOrEmpty(imagePath))
                    {
                        Task task = _sender.SendMessage(type.ToString(), message, botToken, chatId, isSilentNotification, threadId);
                        tasks.Add(task);
                    }
                    else
                    {
                        Task task = _sender.SendMessageWithPhoto(type.ToString(), message, imagePath, botToken, chatId, isSilentNotification, threadId);
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