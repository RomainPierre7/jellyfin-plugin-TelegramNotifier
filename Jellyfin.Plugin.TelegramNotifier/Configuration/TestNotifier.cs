using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Jellyfin.Plugin.TelegramNotifier.Configuration;

[Route("TelegramNotifierApi/TestNotifier")]
[ApiController]
public class TestNotifier : ControllerBase
{
    private readonly Sender _sender;
    private bool result;

    public TestNotifier(Sender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<ActionResult<string>> Get([FromQuery] string botToken, [FromQuery] string chatId, [FromQuery] string? messageTest = null)
    {
        string message = string.IsNullOrEmpty(messageTest) ? "Test" : messageTest!;

        result = await _sender.SendMessage("Test", message, botToken, chatId, false).ConfigureAwait(false);

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
