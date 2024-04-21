using System;
using MediaBrowser.Model.Plugins;

namespace Jellyfin.Plugin.TelegramNotifier.Configuration;

public class PluginConfiguration : BasePluginConfiguration
{
    public PluginConfiguration()
    {
        EnablePlugin = true;
        UserConfigurations = Array.Empty<UserConfiguration>();
    }

    public bool EnablePlugin { get; set; }

    public UserConfiguration[] UserConfigurations { get; set; }
}