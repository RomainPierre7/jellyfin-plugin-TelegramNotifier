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
        string message = "Hi ! This is a message from the TelegramNotifier plugin on your Jellyfin server.\nYour configuration seems correct !";

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
