namespace Jellyfin.Plugin.TelegramNotifier.Configuration
{
    public static class DefaultMessageConfiguration
    {
        static DefaultMessageConfiguration()
        {
            ItemAdded = "ND🆕 {eventArgs.Argument.ItemName} has been added to your library.";
            PlaybackStart = "ND▶️ {eventArgs.Argument.UserName} has started playback of {eventArgs.Argument.ItemName}.";
            PlaybackProgress = "ND⏩ {eventArgs.Argument.UserName} has progressed playback of {eventArgs.Argument.ItemName} to {eventArgs.Argument.PlaybackPosition}.";
            PlaybackStop = "ND⏹️ {eventArgs.Argument.UserName} has stopped playback of {eventArgs.Argument.ItemName}.";
            SubtitleDownloadFailure = "ND❌ Subtitle download failed for {eventArgs.Argument.ItemName}.";
            AuthenticationFailure = "ND🔒 Authentication failed for {eventArgs.Argument.UserName}.";
            AuthenticationSuccess = "ND🔓 Authentication succeeded for {eventArgs.Argument.UserName}.";
            SessionStart = "ND👤 {eventArgs.Argument.UserName} has started a session on:\n" +
                "💻 {eventArgs.Argument.Client} ({eventArgs.Argument.DeviceName})\n";
            PendingRestart = "ND🔄 Jellyfin is pending a restart.";
            TaskCompleted = "ND✅ Task has been completed.";
            PluginInstallationCancelled = "ND❌ Plugin installation has been cancelled.";
            PluginInstallationFailed = "ND❌ Plugin installation has failed.";
            PluginInstalled = "ND✅ Plugin has been installed.";
            PluginInstalling = "ND🔄 Plugin is being installed.";
            PluginUninstalled = "ND❌ Plugin has been uninstalled.";
            PluginUpdated = "ND🔄 Plugin has been updated.";
            UserCreated = "ND👤 User {eventArgs.Argument.UserName} has been created.";
            UserDeleted = "ND👤 User {eventArgs.Argument.UserName} has been deleted.";
            UserLockedOut = "ND🔒 User {eventArgs.Argument.UserName} has been locked out.";
            UserPasswordChanged = "ND🔒 Password for user {eventArgs.Argument.UserName} has been changed.";
            UserUpdated = "ND👤 User {eventArgs.Argument.UserName} has been updated.";
            UserDataSaved = "ND👤 User data for {eventArgs.Argument.UserName} has been saved.";
        }

        public static string ItemAdded { get; }

        public static string PlaybackStart { get; }

        public static string PlaybackProgress { get; }

        public static string PlaybackStop { get; }

        public static string SubtitleDownloadFailure { get; }

        public static string AuthenticationFailure { get; }

        public static string AuthenticationSuccess { get; }

        public static string SessionStart { get; }

        public static string PendingRestart { get; }

        public static string TaskCompleted { get; }

        public static string PluginInstallationCancelled { get; }

        public static string PluginInstallationFailed { get; }

        public static string PluginInstalled { get; }

        public static string PluginInstalling { get; }

        public static string PluginUninstalled { get; }

        public static string PluginUpdated { get; }

        public static string UserCreated { get; }

        public static string UserDeleted { get; }

        public static string UserLockedOut { get; }

        public static string UserPasswordChanged { get; }

        public static string UserUpdated { get; }

        public static string UserDataSaved { get; }
    }
}
