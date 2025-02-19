using DatingTelegramBot.Application.Services.Abstractions.Builder;
using DatingTelegramBot.Application.Services.Abstractions.Commands;
using DatingTelegramBot.Domain.Errors;
using DatingTelegramBot.Domain.Errors.Service.User;
using DatingTelegramBot.Service.Helpers;
using DatingTelegramBot.Service.Services.Commands.Abstract;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace DatingTelegramBot.Service.Services.Commands;

public sealed class DescriptionCommandService(IDescriptionRequestBuilder descriptionRequestBuilder,
    ILogger<BaseDescriptionCommandService> logger)
    : BaseDescriptionCommandService(descriptionRequestBuilder,
        logger), ICommandService<Description>
{
    public async Task<Result<Description, Error>> RetrieveMessageAsync(Update update, string lng)
    {
        if (update.Message.Type == MessageType.Text)
        {
            var (chatId, description) = GetChatIdAndDescription(update);

            var skipTranslation = await TranslatorCommandHelper.GetTranslationAsync(lng, "skip");

            if (description.Contains(skipTranslation, StringComparison.OrdinalIgnoreCase))
            {
                logger.LogInformation("Description from ChatId: {ChatId} contains 'skip'. Skipping.", chatId);
                return new Description("");
            }
            logger.LogInformation("Valid description received from ChatId: {ChatId}.", chatId);
            return new Description(description);
        }
        return UserRegistrationErrors.InvalidMessageTypeError;
    }

    public async Task SendInvalidMessageAsync(Update update, string lng)
    {
        var chatId = update.Message.Chat.Id;
        logger.LogWarning("Sending invalid description message to ChatId: {ChatId}, Language: {Language}", chatId, lng);
        await _descriptionRequestBuilder.SetDescriptionRequest(chatId, lng).SendAsync();
        logger.LogInformation("Invalid description message sent successfully to ChatId: {ChatId}", chatId);
    }

    public async Task SendMessageAsync(Update update, string lng)
    {
        var chatId = update.Message.Chat.Id;
        logger.LogWarning("Sending invalid description message to ChatId: {ChatId}, Language: {Language}", chatId, lng);
        await _descriptionRequestBuilder.SetDescriptionRequest(chatId, lng).SendAsync();
        logger.LogInformation("Invalid description message sent successfully to ChatId: {ChatId}", chatId);
    }
}

public sealed class Description(string? InformationAboutUser)
{
    public override string ToString()
    {
        return InformationAboutUser;
    }
}
