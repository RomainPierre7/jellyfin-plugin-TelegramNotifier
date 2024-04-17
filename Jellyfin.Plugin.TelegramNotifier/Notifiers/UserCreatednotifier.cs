using System.Threading.Tasks;
using Jellyfin.Data.Events.Users;
using MediaBrowser.Controller.Events;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.TelegramNotifier.Notifiers;

public class UserCreatedNotifier : IEventConsumer<UserCreatedEventArgs>
{
    public async Task OnEvent(UserCreatedEventArgs eventArgs)
    {
        ILogger<Plugin> logger = Plugin.Logger;
        logger.LogInformation("User started");

        Sender sender = new Sender();
        await sender.SendMessage("User created").ConfigureAwait(false);
    }
}