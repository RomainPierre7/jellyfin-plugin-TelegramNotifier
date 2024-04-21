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

        public enum NotificationType
        {
            Test,
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

        public async Task Filter(NotificationType type, string message)
        {
            if (Plugin.Config.EnablePlugin == false)
            {
                return;
            }

            string botToken = Plugin.Config.BotToken;
            string chatId = Plugin.Config.ChatId;
            await _sender.SendMessage(type.ToString(), message, botToken, chatId).ConfigureAwait(false);
        }
    }
}