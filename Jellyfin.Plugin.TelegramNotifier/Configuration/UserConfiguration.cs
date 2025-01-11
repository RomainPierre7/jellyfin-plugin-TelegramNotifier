namespace Jellyfin.Plugin.TelegramNotifier.Configuration;

public class UserConfiguration
{
    public UserConfiguration()
    {
        UserId = string.Empty;

        UserName = string.Empty;

        BotToken = string.Empty;

        ChatId = string.Empty;

        ThreadId = string.Empty;

        EnableUser = false;

        SilentNotification = false;

        DoNotMentionOwnActivities = false;

        ItemAdded = false;

        ItemAddedMovies = false;
        ItemAddedMoviesStringMessage = DefaultMessageConfiguration.ItemAddedMovies;

        ItemAddedSeries = false;
        ItemAddedSeriesStringMessage = DefaultMessageConfiguration.ItemAddedSeries;

        ItemAddedSeasons = false;
        ItemAddedSeasonsStringMessage = DefaultMessageConfiguration.ItemAddedSeasons;

        ItemAddedEpisodes = false;
        ItemAddedEpisodesStringMessage = DefaultMessageConfiguration.ItemAddedEpisodes;

        ItemAddedAlbums = false;
        ItemAddedAlbumsStringMessage = DefaultMessageConfiguration.ItemAddedAlbums;

        ItemAddedSongs = false;
        ItemAddedSongsStringMessage = DefaultMessageConfiguration.ItemAddedSongs;

        PlaybackStart = false;
        PlaybackStartStringMessage = DefaultMessageConfiguration.PlaybackStart;

        PlaybackProgress = false;
        PlaybackProgressStringMessage = DefaultMessageConfiguration.PlaybackProgress;

        PlaybackStop = false;
        PlaybackStopStringMessage = DefaultMessageConfiguration.PlaybackStop;

        SubtitleDownloadFailure = false;
        SubtitleDownloadFailureStringMessage = DefaultMessageConfiguration.SubtitleDownloadFailure;

        AuthenticationFailure = false;
        AuthenticationFailureStringMessage = DefaultMessageConfiguration.AuthenticationFailure;

        AuthenticationSuccess = false;
        AuthenticationSuccessStringMessage = DefaultMessageConfiguration.AuthenticationSuccess;

        SessionStart = false;
        SessionStartStringMessage = DefaultMessageConfiguration.SessionStart;

        PendingRestart = false;
        PendingRestartStringMessage = DefaultMessageConfiguration.PendingRestart;

        TaskCompleted = false;
        TaskCompletedStringMessage = DefaultMessageConfiguration.TaskCompleted;

        PluginInstallationCancelled = false;
        PluginInstallationCancelledStringMessage = DefaultMessageConfiguration.PluginInstallationCancelled;

        PluginInstallationFailed = false;
        PluginInstallationFailedStringMessage = DefaultMessageConfiguration.PluginInstallationFailed;

        PluginInstalled = false;
        PluginInstalledStringMessage = DefaultMessageConfiguration.PluginInstalled;

        PluginInstalling = false;
        PluginInstallingStringMessage = DefaultMessageConfiguration.PluginInstalling;

        PluginUninstalled = false;
        PluginUninstalledStringMessage = DefaultMessageConfiguration.PluginUninstalled;

        PluginUpdated = false;
        PluginUpdatedStringMessage = DefaultMessageConfiguration.PluginUpdated;

        UserCreated = false;
        UserCreatedStringMessage = DefaultMessageConfiguration.UserCreated;

        UserDeleted = false;
        UserDeletedStringMessage = DefaultMessageConfiguration.UserDeleted;

        UserLockedOut = false;
        UserLockedOutStringMessage = DefaultMessageConfiguration.UserLockedOut;

        UserPasswordChanged = false;
        UserPasswordChangedStringMessage = DefaultMessageConfiguration.UserPasswordChanged;

        UserUpdated = false;
        UserUpdatedStringMessage = DefaultMessageConfiguration.UserUpdated;

        UserDataSaved = false;
        UserDataSavedStringMessage = DefaultMessageConfiguration.UserDataSaved;
    }

    public string UserId { get; set; }

    public string UserName { get; set; }

    public string BotToken { get; set; }

    public string ChatId { get; set; }

    public string ThreadId { get; set; }

    public bool EnableUser { get; set; }

    public bool SilentNotification { get; set; }

    public bool DoNotMentionOwnActivities { get; set; }

    public bool ItemAdded { get; set; }

    public bool ItemAddedMovies { get; set; }

    public string ItemAddedMoviesStringMessage { get; set; }

    public bool ItemAddedSeries { get; set; }

    public string ItemAddedSeriesStringMessage { get; set; }

    public bool ItemAddedSeasons { get; set; }

    public string ItemAddedSeasonsStringMessage { get; set; }

    public bool ItemAddedEpisodes { get; set; }

    public string ItemAddedEpisodesStringMessage { get; set; }

    public bool ItemAddedAlbums { get; set; }

    public string ItemAddedAlbumsStringMessage { get; set; }

    public bool ItemAddedSongs { get; set; }

    public string ItemAddedSongsStringMessage { get; set; }

    public bool PlaybackStart { get; set; }

    public string PlaybackStartStringMessage { get; set; }

    public bool PlaybackProgress { get; set; }

    public string PlaybackProgressStringMessage { get; set; }

    public bool PlaybackStop { get; set; }

    public string PlaybackStopStringMessage { get; set; }

    public bool SubtitleDownloadFailure { get; set; }

    public string SubtitleDownloadFailureStringMessage { get; set; }

    public bool AuthenticationFailure { get; set; }

    public string AuthenticationFailureStringMessage { get; set; }

    public bool AuthenticationSuccess { get; set; }

    public string AuthenticationSuccessStringMessage { get; set; }

    public bool SessionStart { get; set; }

    public string SessionStartStringMessage { get; set; }

    public bool PendingRestart { get; set; }

    public string PendingRestartStringMessage { get; set; }

    public bool TaskCompleted { get; set; }

    public string TaskCompletedStringMessage { get; set; }

    public bool PluginInstallationCancelled { get; set; }

    public string PluginInstallationCancelledStringMessage { get; set; }

    public bool PluginInstallationFailed { get; set; }

    public string PluginInstallationFailedStringMessage { get; set; }

    public bool PluginInstalled { get; set; }

    public string PluginInstalledStringMessage { get; set; }

    public bool PluginInstalling { get; set; }

    public string PluginInstallingStringMessage { get; set; }

    public bool PluginUninstalled { get; set; }

    public string PluginUninstalledStringMessage { get; set; }

    public bool PluginUpdated { get; set; }

    public string PluginUpdatedStringMessage { get; set; }

    public bool UserCreated { get; set; }

    public string UserCreatedStringMessage { get; set; }

    public bool UserDeleted { get; set; }

    public string UserDeletedStringMessage { get; set; }

    public bool UserLockedOut { get; set; }

    public string UserLockedOutStringMessage { get; set; }

    public bool UserPasswordChanged { get; set; }

    public string UserPasswordChangedStringMessage { get; set; }

    public bool UserUpdated { get; set; }

    public string UserUpdatedStringMessage { get; set; }

    public bool UserDataSaved { get; set; }

    public string UserDataSavedStringMessage { get; set; }
}