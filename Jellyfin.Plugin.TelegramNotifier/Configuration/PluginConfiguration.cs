using System;
using MediaBrowser.Model.Plugins;

namespace Jellyfin.Plugin.TelegramNotifier.Configuration;

public class PluginConfiguration : BasePluginConfiguration
{
    public PluginConfiguration()
    {
        ServerUrl = "localhost:8096";
        EnablePlugin = true;
        UserConfigurations = Array.Empty<UserConfiguration>();
    }

    public string ServerUrl { get; set; }

    public bool EnablePlugin { get; set; }

    public UserConfiguration[] UserConfigurations { get; set; }
}