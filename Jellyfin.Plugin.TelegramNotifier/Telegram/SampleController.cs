using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Jellyfin.Plugin.TelegramNotifier.Telegram;

[Route("api/[controller]")]
[ApiController]
public class SampleController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<string>> Get()
    {
        Sender sender = new Sender();
        string message = "Hi ! This is a message from the TelegramNotifier plugin on your Jellyfin server. Your configuration seems correct";
        await sender.SendMessage(message).ConfigureAwait(false);
        return "Test message successfully executed.";
    }
}
