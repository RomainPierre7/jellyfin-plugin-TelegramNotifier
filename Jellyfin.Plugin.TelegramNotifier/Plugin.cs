using System;
using System.Collections.Generic;
using System.Globalization;
using Jellyfin.Plugin.TelegramNotifier.Configuration;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Model.Plugins;
using MediaBrowser.Model.Serialization;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.TelegramNotifier;

public class Plugin : BasePlugin<PluginConfiguration>, IHasWebPages
{
    private readonly ILogger<Plugin> _logger;

    public Plugin(IApplicationPaths applicationPaths, IXmlSerializer xmlSerializer, ILogger<Plugin> logger)
            : base(applicationPaths, xmlSerializer)
    {
        _logger = logger;
        Instance = this;
    }

    public static ILogger<Plugin> Logger => Instance!._logger;

    public static PluginConfiguration Config => Instance!.Configuration;

    public override string Name => "Telegram Notifier";

    public override Guid Id => Guid.Parse("fd76211d-17e0-4a72-a23f-c6eeb1e48b3a");

    public static Plugin? Instance { get; private set; }

    public IEnumerable<PluginPageInfo> GetPages()
    {
        yield return new PluginPageInfo
        {
            Name = this.Name,
            EmbeddedResourcePath = string.Format(CultureInfo.InvariantCulture, "{0}.Configuration.Web.configPage.html", GetType().Namespace)
        };
        yield return new PluginPageInfo
        {
            Name = $"{this.Name}.js",
            EmbeddedResourcePath = string.Format(CultureInfo.InvariantCulture, "{0}.Configuration.Web.configPage.js", GetType().Namespace)
        };
    }
}