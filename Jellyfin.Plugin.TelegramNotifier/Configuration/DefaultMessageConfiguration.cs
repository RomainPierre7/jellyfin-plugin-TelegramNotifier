using System.Reflection.Metadata.Ecma335;
using MediaBrowser.Model.Session;

namespace Jellyfin.Plugin.TelegramNotifier.Configuration
{
    public static class DefaultMessageConfiguration
    {
        static DefaultMessageConfiguration()
        {
            ItemAddedBooks = "[Book] {item.Name}\n" +
                " added to library\n\n" +
                "{item.Overview}";

            ItemAddedMovies = "🎬 {item.Name} ({item.ProductionYear})\n" +
                "      added to library\n\n" +
                "📽 {item.Overview}";

            ItemAddedSeries = "📺 [Serie] {serie.Name} ({item.ProductionYear}) added to library\n\n" +
                "📽 {item.Overview}";

            ItemAddedSeasons = "📺 {season.Series.Name} ({item.ProductionYear})\n" +
                "      Season {seasonNumber} added to library\n\n" +
                "📽 {item.Overview}";

            ItemAddedEpisodes = "📺 {episode.Series.Name} ({item.ProductionYear})\n" +
                "      S{eSeasonNumber} - E{episodeNumber}\n" +
                "      '{item.Name}' added to library\n\n" +
                "📽 {item.Overview}";

            ItemAddedAlbums = "🎵 [Album] {album.Name} ({item.ProductionYear}) added to library";

            ItemAddedSongs = "🎵 [Audio] {audio.Name} ({item.ProductionYear}) added to library";

            AuthenticationFailure = "🔒 Authentication failure on {eventArgs.Argument.DeviceName} for user {eventArgs.Argument.Username}";

            AuthenticationSuccess = "🔓 Authentication success for user {eventArgs.Argument.User.Name} on {eventArgs.Argument.SessionInfo.DeviceName}";

            PendingRestart = "🔄 Jellyfin is pending a restart.";

            PlaybackProgressMovies = "👤 {eventArgs.Users[0].Username} is still watching on {eventArgs.DeviceName}:\n" +
                "🎬 {eventArgs.Item.Name} ({eventArgs.Item.ProductionYear})";

            PlaybackProgressEpisodes = "👤 {eventArgs.Users[0].Username} is still watching on {eventArgs.DeviceName}:\n" +
                "🎬 {eventArgs.Item.Series.Name} ({eventArgs.Item.ProductionYear})\n" +
                "      S{playbackSeasonNumber} - E{playbackEpisodeNumber}\n" +
                "      '{eventArgs.Item.Name}'";

            PlaybackStartMovies = "👤 {eventArgs.Users[0].Username} is watching on {eventArgs.DeviceName}:\n" +
                "🎬 {eventArgs.Item.Name} ({eventArgs.Item.ProductionYear})\n" +
                "📺 [{eventArgs.Item.MediaType}] {eventArgs.Item.Genres}\n" +
                "🕒 {duration}\n" +
                "📽 {eventArgs.Item.Overview}";

            PlaybackStartEpisodes = "👤 {eventArgs.Users[0].Username} is watching on {eventArgs.DeviceName}:\n" +
                "🎬 {eventArgs.Item.Series.Name} ({eventArgs.Item.ProductionYear})\n" +
                "      S{playbackSeasonNumber} - E{playbackEpisodeNumber}\n" +
                "      '{eventArgs.Item.Name}'\n" +
                "📺 [{eventArgs.Item.MediaType}] {eventArgs.Item.Genres}\n" +
                "🕒 {duration}\n" +
                "📽 {eventArgs.Item.Overview}";

            PlaybackStopMovies = "👤 {eventArgs.Users[0].Username} stopped watching:\n" +
                "🎬 {eventArgs.Item.Name} ({eventArgs.Item.ProductionYear})";

            PlaybackStopEpisodes = "👤 {eventArgs.Users[0].Username} stopped watching:\n" +
                "🎬 {eventArgs.Item.Series.Name} ({eventArgs.Item.ProductionYear})\n" +
                "      S{playbackSeasonNumber} - E{playbackEpisodeNumber}\n" +
                "      '{eventArgs.Item.Name}'";

            PluginInstallationCancelled = "🔴 {eventArgs.Argument.Name} plugin installation cancelled (version {eventArgs.Argument.Version}):";

            PluginInstallationFailed = "🔴 {eventArgs.InstallationInfo} plugin installation failed (version {eventArgs.VersionInfo}):\n" +
                "{eventArgs.Exception}";

            PluginInstalled = "🚧 {eventArgs.Argument.Name} plugin installed (version {eventArgs.Argument.Version})";

            PluginInstalling = "🚧 {eventArgs.Argument.Name} plugin is installing (version {eventArgs.Argument.Version})";

            PluginUninstalled = "🚧 {eventArgs.Argument.Name} plugin uninstalled";

            PluginUpdated = "🚧 {eventArgs.Argument.Name} plugin updated to version {eventArgs.Argument.Version}:" +
                "🗒️ {eventArgs.Argument.Changelog}\n\n" +
                "You may need to restart Jellyfin to apply the changes.";

            SessionStart = "👤 {eventArgs.Argument.UserName} has started a session on:\n" +
                "💻 {eventArgs.Argument.Client} ({eventArgs.Argument.DeviceName})\n";

            SubtitleDownloadFailure = "🚫 Subtitle download failed for {eventArgs.Item.Name}";

            TaskCompleted = "🧰 Task {eventArgs.Task.Name} completed: {eventArgs.Task.CurrentProgress}%\n" +
                "🗒️ ({eventArgs.Task.Category}) {eventArgs.Task.Description}";

            UserCreated = "👤 User {eventArgs.Argument.Username} created.";

            UserDeleted = "🗑️ User {eventArgs.Argument.Username} deleted.";

            UserLockedOut = "👤🔒 User {eventArgs.Argument.Username} locked out";

            UserPasswordChanged = "👤 User {eventArgs.Argument.Username} changed his password.";

            UserUpdated = "👤 User {eventArgs.Argument.Username} has been updated";

            UserDataSaved = "👤 User {eventArgs.Argument.Username} data saved.";
        }

        public static string ItemAddedBooks { get; }

        public static string ItemAddedMovies { get; }

        public static string ItemAddedSeries { get; }

        public static string ItemAddedSeasons { get; }

        public static string ItemAddedEpisodes { get; }

        public static string ItemAddedAlbums { get; }

        public static string ItemAddedSongs { get; }

        public static string PlaybackStartMovies { get; }

        public static string PlaybackStartEpisodes { get; }

        public static string PlaybackProgressMovies { get; }

        public static string PlaybackProgressEpisodes { get; }

        public static string PlaybackStopMovies { get; }

        public static string PlaybackStopEpisodes { get; }

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
