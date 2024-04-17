using System;
using System.Threading.Tasks;
using MediaBrowser.Controller.Events;
using MediaBrowser.Controller.Library;

namespace Jellyfin.Plugin.TelegramNotifier.Notifiers;

public class PlaybackStopNotifier : IEventConsumer<PlaybackStopEventArgs>
{
    private readonly Sender _sender;

    public PlaybackStopNotifier(Sender sender)
    {
        _sender = sender;
    }

    public async Task OnEvent(PlaybackStopEventArgs eventArgs)
    {
        if (eventArgs == null)
        {
            throw new ArgumentNullException(nameof(eventArgs));
        }

        if (eventArgs.Item is null || eventArgs.Users.Count == 0 || eventArgs.Item.IsThemeMedia)
        {
            return;
        }

        string message = $"👤 {eventArgs.Users[0].Username} stopped watching {eventArgs.Item.Name} ({eventArgs.Item.ProductionYear})";

        await _sender.SendMessage(message, logEvent: "Playback Stoped").ConfigureAwait(false);
    }
}