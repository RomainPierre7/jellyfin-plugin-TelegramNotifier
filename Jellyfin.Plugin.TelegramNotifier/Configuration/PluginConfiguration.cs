using System;
using MediaBrowser.Model.Plugins;

namespace Jellyfin.Plugin.TelegramNotifier.Configuration;

public class PluginConfiguration : BasePluginConfiguration
{
    public PluginConfiguration()
    {
        EnablePlugin = true;
        BotToken = string.Empty;
        ChatId = string.Empty;
        UserConfigurations = Array.Empty<UserConfiguration>();
    }

    public bool EnablePlugin { get; set; }

    public string BotToken { get; set; }

    public string ChatId { get; set; }

    public UserConfiguration[] UserConfigurations { get; set; }
}