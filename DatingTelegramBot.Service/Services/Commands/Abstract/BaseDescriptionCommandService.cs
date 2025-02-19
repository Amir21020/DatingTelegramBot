using DatingTelegramBot.Application.Services.Abstractions.Builder;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;

namespace DatingTelegramBot.Service.Services.Commands.Abstract;

public abstract  class BaseDescriptionCommandService(IDescriptionRequestBuilder descriptionRequestBuilder,
    ILogger<BaseDescriptionCommandService> logger)
{
    protected readonly ILogger<BaseDescriptionCommandService> _logger = logger;
    protected readonly IDescriptionRequestBuilder _descriptionRequestBuilder = descriptionRequestBuilder;
    protected (long chatId, string description) GetChatIdAndDescription(Update update)
    {
        var chatId = update.Message.Chat.Id;
        var description = update.Message.Text;
        logger.LogInformation("Received description from ChatId: {ChatId}, Description: {Description}", chatId, description);
        return (chatId, description);
    }
}
