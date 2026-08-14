using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jellyfin.Plugin.TelegramNotifier.Configuration;
using Jellyfin.Plugin.TelegramNotifier.Notifiers.ItemAddedNotifier;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Entities.TV;
using MediaBrowser.Controller.Library;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.TelegramNotifier
{
    public class NotificationFilter
    {
        private readonly Sender _sender;
        private readonly ILogger<Plugin> _logger;
        private readonly ILibraryManager _libraryManager;
        private readonly IMediaSourceManager _mediaSourceManager;
        private readonly ItemNotificationStore _itemNotificationStore;

        public NotificationFilter(
            Sender sender,
            ILibraryManager libraryManager,
            IMediaSourceManager mediaSourceManager,
            ItemNotificationStore itemNotificationStore)
        {
            _sender = sender;
            _libraryManager = libraryManager;
            _mediaSourceManager = mediaSourceManager;
            _itemNotificationStore = itemNotificationStore;
            _logger = Plugin.Logger;
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

        public async Task<IReadOnlyList<ItemNotificationRecord>> Filter(NotificationType type, dynamic eventArgs, string userId = "", string imagePath = "", string subtype = "", Guid? trackedItemId = null)
        {
            if (!Plugin.Config.EnablePlugin)
            {
                return Array.Empty<ItemNotificationRecord>();
            }

            UserConfiguration[] users = Plugin.Config.UserConfigurations;
            var tasks = new List<Task<ItemNotificationRecord?>>();

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

                message = MessageParser.ParseMessage(message, eventArgs, _libraryManager, _mediaSourceManager);

                string botToken = user.BotToken;
                string chatId = user.ChatId;
                bool isSilentNotification = user.SilentNotification;
                string threadId = user.ThreadId;

                /* ---------- Avoid duplicated notifications ---------- */
                string fingerprint = $"{type}|{chatId}|{message}";

                if (!NotificationDeduplicator.ShouldSend(fingerprint))
                {
                    _logger.LogInformation(
                        "Duplicate notification skipped ({NotificationType}|{ChatId}|{Message})", type, chatId, message);
                    continue;
                }

                try
                {
                    if (string.IsNullOrEmpty(imagePath))
                    {
                        Task<ItemNotificationRecord?> task = SendAndTrackAsync(
                            type.ToString(),
                            message,
                            botToken,
                            chatId,
                            threadId,
                            user.UserId ?? string.Empty,
                            isSilentNotification,
                            string.Empty,
                            trackedItemId);
                        tasks.Add(task);
                    }
                    else
                    {
                        string notificationImagePath = imagePath;
                        Episode episode = null;

                        // Case 1 : eventArgs is an Episode
                        if (eventArgs is Episode ep)
                        {
                            episode = ep;
                        }
                        // Case 2 : eventArgs contains Item
                        else
                        {
                            try
                            {
                                if (eventArgs?.Item is Episode ep2)
                                {
                                    episode = ep2;
                                }
                            }
                            catch
                            {
                                // Ignore if not item property
                            }
                        }

                        if (user.KeepSerieImage && episode != null)
                        {
                            string serverUrl = Plugin.Instance?.Configuration.ServerUrl ?? "localhost:8096";
                            notificationImagePath = "http://" + serverUrl + "/Items/" + episode.Series.Id + "/Images/Primary";
                        }

                        Task<ItemNotificationRecord?> task = SendAndTrackAsync(
                            type.ToString(),
                            message,
                            botToken,
                            chatId,
                            threadId,
                            user.UserId ?? string.Empty,
                            isSilentNotification,
                            notificationImagePath,
                            trackedItemId);
                        tasks.Add(task);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while sending a message");
                }
            }

            ItemNotificationRecord?[] results = await Task.WhenAll(tasks).ConfigureAwait(false);
            return results.Where(result => result is not null).Cast<ItemNotificationRecord>().ToArray();
        }

        public async Task UpdateItemAddedNotifications(BaseItem item, string subtype, IReadOnlyList<ItemNotificationRecord> records)
        {
            foreach (ItemNotificationRecord record in records)
            {
                UserConfiguration? user = Plugin.Config.UserConfigurations.FirstOrDefault(candidate =>
                    candidate.EnableUser &&
                    candidate.ItemAdded &&
                    candidate.ChatId == record.ChatId &&
                    candidate.ThreadId == record.ThreadId &&
                    (string.IsNullOrEmpty(record.UserId) || candidate.UserId == record.UserId));

                if (user is null || !GetPropertyValue(user, subtype))
                {
                    continue;
                }

                string template = GetPropertyMessage(user, subtype);
                string message = MessageParser.ParseMessage(template, item, _libraryManager, _mediaSourceManager);
                if (message == record.RenderedMessage)
                {
                    continue;
                }

                bool updated = record.HasPhoto
                    ? await _sender.EditMessageCaption(NotificationType.ItemAdded.ToString(), message, user.BotToken, record.ChatId, record.MessageId).ConfigureAwait(false)
                    : await _sender.EditMessageText(NotificationType.ItemAdded.ToString(), message, user.BotToken, record.ChatId, record.MessageId).ConfigureAwait(false);

                if (updated)
                {
                    record.RenderedMessage = message;
                    await _itemNotificationStore.UpdateAsync(record).ConfigureAwait(false);
                }
            }
        }

        private async Task<ItemNotificationRecord?> SendAndTrackAsync(
            string notificationType,
            string message,
            string botToken,
            string chatId,
            string threadId,
            string userId,
            bool isSilentNotification,
            string imagePath,
            Guid? trackedItemId)
        {
            TelegramMessageResult? result = string.IsNullOrEmpty(imagePath)
                ? await _sender.SendMessage(notificationType, message, botToken, chatId, isSilentNotification, threadId).ConfigureAwait(false)
                : await _sender.SendMessageWithPhoto(notificationType, message, imagePath, botToken, chatId, isSilentNotification, threadId).ConfigureAwait(false);

            if (result is null || trackedItemId is null)
            {
                return null;
            }

            return new ItemNotificationRecord
            {
                ItemId = trackedItemId.Value,
                UserId = userId,
                ChatId = chatId,
                ThreadId = threadId,
                MessageId = result.MessageId,
                HasPhoto = !string.IsNullOrEmpty(imagePath),
                RenderedMessage = message,
                SentAtUtc = DateTime.UtcNow
            };
        }
    }
}
