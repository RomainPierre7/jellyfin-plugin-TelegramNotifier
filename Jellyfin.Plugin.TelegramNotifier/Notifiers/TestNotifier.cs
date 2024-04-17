using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Jellyfin.Plugin.TelegramNotifier.Telegram;

[Route("TelegramNotifierApi/TestNotifier")]
[ApiController]
public class TestNotifier : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<string>> Get()
    {
        Sender sender = new Sender();
        string message = "[Jellyfin] Test message: \n 🎉 Your configuration is correct ! 🥳";

        bool result = await sender.SendMessage(message).ConfigureAwait(false);

        if (result)
        {
            return Ok("Message sent successfully");
        }
        else
        {
            return BadRequest("Message could not be sent, please check your configuration");
        }
    }
}
