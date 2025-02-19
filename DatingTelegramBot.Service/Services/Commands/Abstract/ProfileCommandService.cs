using DatingTelegramBot.Application.Services.Abstractions.Builder;
using DatingTelegramBot.Service.Helpers;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;

namespace DatingTelegramBot.Service.Services.Commands.Abstract;

public abstract class ProfileCommandService(IDescriptionRequestBuilder descriptionRequestBuilder,
    IMediaRequestBuilder mediaRequestBuilder, ILogger<ProfileCommandService> logger)
{
    protected readonly IDescriptionRequestBuilder _descriptionRequestBuilder = descriptionRequestBuilder;
    protected readonly IMediaRequestBuilder _mediaRequestBuilder = mediaRequestBuilder;
    protected async Task<string> RequestAndGetUserDescriptionAsync(Update update, string lng)
    {
        var chatId = update.Message.Chat.Id;
        logger.LogInformation("Requesting description for ChatId: {ChatId}, Language: {Language}", chatId, lng);

        await _descriptionRequestBuilder.SetDescriptionRequest(chatId, lng).SendAsync();

        var descriptions = update.Message.Text;

        logger.LogInformation("Received descriptions from user: {Descriptions}", descriptions);

        if (descriptions.Contains(await TranslatorCommandHelper.GetTranslationAsync(lng, "skip"), StringComparison.OrdinalIgnoreCase))
        {
            logger.LogInformation("User opted to skip description for ChatId: {ChatId}", chatId);
            return null;
        }

        logger.LogInformation("User provided description for ChatId: {ChatId}: {Description}", chatId, descriptions);
        return descriptions;
    }

}
