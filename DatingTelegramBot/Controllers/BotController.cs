using DatingTelegramBot.Application.Services.Abstractions.Telegram;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace DatingTelegramBot.Controllers;

[Route("[controller]")]
[ApiController]
public sealed class BotController(ICommandExecutor commandExecutor,
    ILogger<BotController> logger) : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Started");
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Update upd)
    {
        if (upd?.Message?.Chat == null && upd?.CallbackQuery == null)
        {
            logger.LogWarning("Received an update with no relevant data.");
            return Ok();
        }
        logger.LogInformation("Processing update: {UpdateId}", upd.Id);
        await commandExecutor.ExecuteAsync(upd);
        return Ok();
    }
}
