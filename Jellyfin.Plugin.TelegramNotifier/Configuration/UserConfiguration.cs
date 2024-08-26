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
        ItemAddedSeries = false;
        ItemAddedSeasons = false;
        ItemAddedEpisodes = false;
        ItemAddedAlbums = false;
        ItemAddedSongs = false;

        Generic = false;

        PlaybackStart = false;

        PlaybackProgress = false;

        PlaybackStop = false;

        SubtitleDownloadFailure = false;

        AuthenticationFailure = false;

        AuthenticationSuccess = false;

        SessionStart = false;

        PendingRestart = false;

        TaskCompleted = false;

        PluginInstallationCancelled = false;

        PluginInstallationFailed = false;

        PluginInstalled = false;

        PluginInstalling = false;

        PluginUninstalled = false;

        PluginUpdated = false;

        UserCreated = false;

        UserDeleted = false;

        UserLockedOut = false;

        UserPasswordChanged = false;

        UserUpdated = false;

        UserDataSaved = false;
    }

    public string? UserId { get; set; }

    public string? UserName { get; set; }

    public string? BotToken { get; set; }

    public string? ChatId { get; set; }

    public string? ThreadId { get; set; }

    public bool? EnableUser { get; set; }

    public bool? SilentNotification { get; set; }

    public bool? DoNotMentionOwnActivities { get; set; }

    public bool? ItemAdded { get; set; }

    public bool? ItemAddedMovies { get; set; }

    public bool? ItemAddedSeries { get; set; }

    public bool? ItemAddedSeasons { get; set; }

    public bool? ItemAddedEpisodes { get; set; }

    public bool? ItemAddedAlbums { get; set; }

    public bool? ItemAddedSongs { get; set; }

    public bool? Generic { get; set; }

    public bool? PlaybackStart { get; set; }

    public bool? PlaybackProgress { get; set; }

    public bool? PlaybackStop { get; set; }

    public bool? SubtitleDownloadFailure { get; set; }

    public bool? AuthenticationFailure { get; set; }

    public bool? AuthenticationSuccess { get; set; }

    public bool? SessionStart { get; set; }

    public bool? PendingRestart { get; set; }

    public bool? TaskCompleted { get; set; }

    public bool? PluginInstallationCancelled { get; set; }

    public bool? PluginInstallationFailed { get; set; }

    public bool? PluginInstalled { get; set; }

    public bool? PluginInstalling { get; set; }

    public bool? PluginUninstalled { get; set; }

    public bool? PluginUpdated { get; set; }

    public bool? UserCreated { get; set; }

    public bool? UserDeleted { get; set; }

    public bool? UserLockedOut { get; set; }

    public bool? UserPasswordChanged { get; set; }

    public bool? UserUpdated { get; set; }

    public bool? UserDataSaved { get; set; }
}