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

        ItemAddedMovies = true;
        ItemAddedMoviesStringMessage = DefaultMessageConfiguration.ItemAddedMovies;

        ItemAddedSeries = true;
        ItemAddedSeriesStringMessage = DefaultMessageConfiguration.ItemAddedSeries;

        ItemAddedSeasons = true;
        ItemAddedSeasonsStringMessage = DefaultMessageConfiguration.ItemAddedSeasons;

        ItemAddedEpisodes = true;
        ItemAddedEpisodesStringMessage = DefaultMessageConfiguration.ItemAddedEpisodes;

        ItemAddedAlbums = true;
        ItemAddedAlbumsStringMessage = DefaultMessageConfiguration.ItemAddedAlbums;

        ItemAddedSongs = true;
        ItemAddedSongsStringMessage = DefaultMessageConfiguration.ItemAddedSongs;

        ItemAddedBooks = true;
        ItemAddedBooksStringMessage = DefaultMessageConfiguration.ItemAddedBooks;

        ItemDeleted = false;

        ItemDeletedMovies = true;
        ItemDeletedMoviesStringMessage = DefaultMessageConfiguration.ItemDeletedMovies;

        ItemDeletedSeries = true;
        ItemDeletedSeriesStringMessage = DefaultMessageConfiguration.ItemDeletedSeries;

        ItemDeletedSeasons = true;
        ItemDeletedSeasonsStringMessage = DefaultMessageConfiguration.ItemDeletedSeasons;

        ItemDeletedEpisodes = true;
        ItemDeletedEpisodesStringMessage = DefaultMessageConfiguration.ItemDeletedEpisodes;

        ItemDeletedAlbums = true;
        ItemDeletedAlbumsStringMessage = DefaultMessageConfiguration.ItemDeletedAlbums;

        ItemDeletedSongs = true;
        ItemDeletedSongsStringMessage = DefaultMessageConfiguration.ItemDeletedSongs;

        ItemDeletedBooks = true;
        ItemDeletedBooksStringMessage = DefaultMessageConfiguration.ItemDeletedBooks;

        PlaybackStart = false;

        PlaybackStartMovies = true;
        PlaybackStartMoviesStringMessage = DefaultMessageConfiguration.PlaybackStartMovies;

        PlaybackStartEpisodes = true;
        PlaybackStartEpisodesStringMessage = DefaultMessageConfiguration.PlaybackStartEpisodes;

        PlaybackProgress = false;

        PlaybackProgressMovies = true;
        PlaybackProgressMoviesStringMessage = DefaultMessageConfiguration.PlaybackProgressMovies;

        PlaybackProgressEpisodes = true;
        PlaybackProgressEpisodesStringMessage = DefaultMessageConfiguration.PlaybackProgressEpisodes;

        PlaybackStop = false;

        PlaybackStopMovies = true;
        PlaybackStopMoviesStringMessage = DefaultMessageConfiguration.PlaybackStopMovies;

        PlaybackStopEpisodes = true;
        PlaybackStopEpisodesStringMessage = DefaultMessageConfiguration.PlaybackStopEpisodes;

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

    public bool ItemAddedBooks { get; set; }

    public string ItemAddedBooksStringMessage { get; set; }

    public bool ItemDeleted { get; set; }

    public bool ItemDeletedMovies { get; set; }

    public string ItemDeletedMoviesStringMessage { get; set; }

    public bool ItemDeletedSeries { get; set; }

    public string ItemDeletedSeriesStringMessage { get; set; }

    public bool ItemDeletedSeasons { get; set; }

    public string ItemDeletedSeasonsStringMessage { get; set; }

    public bool ItemDeletedEpisodes { get; set; }

    public string ItemDeletedEpisodesStringMessage { get; set; }

    public bool ItemDeletedAlbums { get; set; }

    public string ItemDeletedAlbumsStringMessage { get; set; }

    public bool ItemDeletedSongs { get; set; }

    public string ItemDeletedSongsStringMessage { get; set; }

    public bool ItemDeletedBooks { get; set; }

    public string ItemDeletedBooksStringMessage { get; set; }

    public bool PlaybackStart { get; set; }

    public bool PlaybackStartMovies { get; set; }

    public string PlaybackStartMoviesStringMessage { get; set; }

    public bool PlaybackStartEpisodes { get; set; }

    public string PlaybackStartEpisodesStringMessage { get; set; }

    public bool PlaybackProgress { get; set; }

    public bool PlaybackProgressMovies { get; set; }

    public string PlaybackProgressMoviesStringMessage { get; set; }

    public bool PlaybackProgressEpisodes { get; set; }

    public string PlaybackProgressEpisodesStringMessage { get; set; }

    public bool PlaybackStop { get; set; }

    public bool PlaybackStopMovies { get; set; }

    public string PlaybackStopMoviesStringMessage { get; set; }

    public bool PlaybackStopEpisodes { get; set; }

    public string PlaybackStopEpisodesStringMessage { get; set; }

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