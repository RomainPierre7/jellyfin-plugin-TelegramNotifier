using MediaBrowser.Model.Plugins;

namespace Jellyfin.Plugin.TelegramNotifier.Configuration;

public class PluginConfiguration : BasePluginConfiguration
{
    public PluginConfiguration()
    {
        // set default options here
        EnablePlugin = true;
        BotToken = string.Empty;
        ChatId = string.Empty;
    }

    public bool EnablePlugin { get; set; }

    public string BotToken { get; set; }

    public string ChatId { get; set; }
}