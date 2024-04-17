using System.Collections.Generic;
using System.Threading.Tasks;
using Jellyfin.Plugin.TelegramNotifier;
using MediaBrowser.Controller;
using MediaBrowser.Controller.Events;
using MediaBrowser.Controller.Library;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.Webhook.Notifiers;

/// <summary>
/// Playback start notifier.
/// </summary>
public class PlaybackStartNotifier : IEventConsumer<PlaybackStartEventArgs>
{
    private readonly IServerApplicationHost _applicationHost;

    /// <summary>
    /// Initializes a new instance of the <see cref="PlaybackStartNotifier"/> class.
    /// </summary>
    /// <param name="applicationHost">Instance of the <see cref="IServerApplicationHost"/> interface.</param>
    public PlaybackStartNotifier(
        IServerApplicationHost applicationHost)
    {
        _applicationHost = applicationHost;
    }

    /// <inheritdoc />
    public async Task OnEvent(PlaybackStartEventArgs eventArgs)
    {
        if (eventArgs.Item is null)
        {
            return;
        }

        if (eventArgs.Item.IsThemeMedia)
        {
            // Don't report theme song or local trailer playback.
            return;
        }

        if (eventArgs.Users.Count == 0)
        {
            // No users in playback session.
            return;
        }

        string message = $"🎬 {eventArgs.Item.Name} ({eventArgs.Item.ProductionYear})\n" +
                         $"📺 {eventArgs.Item.MediaType}\n" +
                         $"🕒 {eventArgs.Item.RunTimeTicks / 600000000} minutes\n" +
                         $"📽 {eventArgs.Item.Overview}";

        Sender sender = new Sender();
        await sender.SendMessage(message).ConfigureAwait(false);
    }
}