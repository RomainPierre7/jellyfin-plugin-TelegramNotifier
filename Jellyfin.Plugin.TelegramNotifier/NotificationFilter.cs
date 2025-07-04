using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jellyfin.Plugin.TelegramNotifier.Configuration;
using MediaBrowser.Controller.Entities.TV;

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
            ItemDeleted,
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

        private string GetPropertyMessage(UserConfiguration user, string propertyName)
        {
            var property_message = user.GetType().GetProperty(propertyName + "StringMessage");
            if (property_message != null)
            {
                var message = property_message.GetValue(user);
                if (message != null)
                {
                    return (string)message;
                }
                else
                {
                    throw new ArgumentException($"The property {propertyName + "StringMessage"} is null.");
                }
            }
            else
            {
                throw new ArgumentException($"The property {propertyName + "StringMessage"} does not exist.");
            }
        }

        public async Task Filter(NotificationType type, dynamic eventArgs, string userId = "", string imagePath = "", string subtype = "")
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

                if (user.DoNotMentionOwnActivities == true && user.UserId is not null)
                {
                    string currentUserid = user.UserId.Replace("-", string.Empty, StringComparison.OrdinalIgnoreCase);
                    string notifUserId = userId.Replace("-", string.Empty, StringComparison.OrdinalIgnoreCase);
                    if (currentUserid == notifUserId)
                    {
                        continue;
                    }
                }

                string message;

                if (!string.IsNullOrEmpty(subtype))
                {
                    bool isSubTypeEnabled = GetPropertyValue(user, subtype);
                    if (!isSubTypeEnabled)
                    {
                        continue;
                    }

                    message = GetPropertyMessage(user, subtype);
                }
                else
                {
                    message = GetPropertyMessage(user, type.ToString());
                }

                message = MessageParser.ParseMessage(message, eventArgs);

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
                        if (user.KeepSerieImage && eventArgs.Item is Episode)
                        {
                            var episode = (Episode)eventArgs.Item;
                            string serverUrl = Plugin.Instance?.Configuration.ServerUrl ?? "localhost:8096";
                            imagePath = "http://" + serverUrl + "/Items/" + episode.Series.Id + "/Images/Primary";
                        }
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