using System;
using System.Threading.Tasks;
using Jellyfin.Plugin.TelegramNotifier.Configuration;
using Microsoft.Extensions.Configuration;

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

            UserConfiguration[] users = Plugin.Config.UserConfigurations;
            foreach (UserConfiguration user in users)
            {
                if (user.EnableUser == false)
                {
                    continue;
                }

                string botToken = user.BotToken ?? string.Empty;
                string chatId = user.ChatId ?? string.Empty;
                await _sender.SendMessage(type.ToString(), message, botToken, chatId).ConfigureAwait(false);
            }
        }
    }
}