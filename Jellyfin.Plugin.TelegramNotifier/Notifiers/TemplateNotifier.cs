/* using System;
using System.Threading.Tasks;
using Jellyfin.Data.Events.Templates;
using MediaBrowser.Controller.Events;

namespace Jellyfin.Plugin.TelegramNotifier.Notifiers;

public class TemplateCreatedNotifier : IEventConsumer<TemplateCreatedEventArgs>
{
    private readonly Sender _sender;

    public TemplateCreatedNotifier(Sender sender)
    {
        _sender = sender;
    }

    public async Task OnEvent(TemplateCreatedEventArgs eventArgs)
    {
        if (eventArgs == null)
        {
            throw new ArgumentNullException(nameof(eventArgs));
        }

        string message = $"🎉 New Template created: {eventArgs.Argument.Templatename}";

        await _sender.SendMessage(message, logEvent: "Template created").ConfigureAwait(false);
    }
} */