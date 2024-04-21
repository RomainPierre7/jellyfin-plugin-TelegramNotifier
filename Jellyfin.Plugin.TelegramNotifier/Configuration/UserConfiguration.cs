namespace Jellyfin.Plugin.TelegramNotifier.Configuration;

public class UserConfiguration
{
    public string? UserId { get; set; }

    public string? UserName { get; set; }

    public string? BotToken { get; set; }

    public string? ChatId { get; set; }

    public bool? EnableUser { get; set; }
}
