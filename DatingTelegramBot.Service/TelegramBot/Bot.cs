using DatingTelegramBot.Domain.Abstractions;
using DatingTelegramBot.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace DatingTelegramBot.Service.TelegramBot;

public sealed class Bot
    (IOptions<TokenOptions> options,
    ILogger<Bot> logger) : IBot
{
    private readonly TokenOptions _token = options.Value;
    private TelegramBotClient _botClient;
    public async Task<TelegramBotClient> GetTelegramBot()
    {
        if (_botClient != null)
        {
            logger.LogInformation("Returning existing TelegramBotClient instance.");
            return _botClient;
        }
        logger.LogInformation("Creating new TelegramBotClient instance.");
        _botClient = new TelegramBotClient(_token.Token);
        logger.LogInformation("TelegramBotClient created successfully.");
        return _botClient;
    }
}
