namespace Jellyfin.Plugin.TelegramNotifier.Configuration;

public class UserConfiguration
{
    public string? UserId { get; set; }

    public string? UserName { get; set; }

    public string? BotToken { get; set; }

    public string? ChatId { get; set; }

    public bool? EnableUser { get; set; }

    public bool? Test { get; set; }

    public bool? ItemAdded { get; set; }

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
