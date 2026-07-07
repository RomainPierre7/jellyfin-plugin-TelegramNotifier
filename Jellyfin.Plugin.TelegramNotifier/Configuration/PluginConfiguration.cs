using System;
using MediaBrowser.Model.Plugins;

namespace Jellyfin.Plugin.TelegramNotifier.Configuration;

public class PluginConfiguration : BasePluginConfiguration
{
    public PluginConfiguration()
    {
        ServerUrl = "localhost:8096";
        EnablePlugin = true;
        EmptyPlaceholderReplacement = "...";
        UserConfigurations = Array.Empty<UserConfiguration>();
    }

    public string ServerUrl { get; set; }

    public string ServerDisplayUrl { get; set; }

    public bool EnablePlugin { get; set; }

    /// <summary>
    /// Gets or sets the replacement for empty placeholders: "" (blank), "...", "N/A", or "—".
    /// </summary>
    public string EmptyPlaceholderReplacement { get; set; }

    public UserConfiguration[] UserConfigurations { get; set; }
}