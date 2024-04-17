using System;
using System.Threading.Tasks;
using Jellyfin.Data.Events.Users;
using MediaBrowser.Controller.Events;

namespace Jellyfin.Plugin.TelegramNotifier.Notifiers;

public class UserCreatedNotifier : IEventConsumer<UserCreatedEventArgs>
{
    private readonly Sender _sender;

    public UserCreatedNotifier(Sender sender)
    {
        _sender = sender;
    }

    public async Task OnEvent(UserCreatedEventArgs eventArgs)
    {
        if (eventArgs == null)
        {
            throw new ArgumentNullException(nameof(eventArgs));
        }

        string message = $"👤 New user created: {eventArgs.Argument.Username}";

        await _sender.SendMessage(message, logEvent: "User created").ConfigureAwait(false);
    }
}