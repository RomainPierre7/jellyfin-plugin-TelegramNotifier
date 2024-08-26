using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Jellyfin.Plugin.TelegramNotifier.Telegram.Configuration;

[Route("TelegramNotifierApi/TestNotifier")]
[ApiController]
public class TestNotifier : ControllerBase
{
    private readonly Sender _sender;

    public TestNotifier(Sender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<ActionResult<string>> Get([FromQuery] string botToken, [FromQuery] string chatId, [FromQuery] string threadId)
    {
        string message = "[Jellyfin] Test message: \n 🎉 Your configuration is correct ! 🥳";

        bool result = await _sender.SendMessage("Test", message, botToken, chatId, false, threadId).ConfigureAwait(false);

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
