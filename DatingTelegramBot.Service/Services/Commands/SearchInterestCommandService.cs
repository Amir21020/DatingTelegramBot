using DatingTelegramBot.Application.Services.Abstractions.Builder;
using DatingTelegramBot.Application.Services.Abstractions.Commands;
using DatingTelegramBot.Domain.Enum;
using DatingTelegramBot.Domain.Errors;
using DatingTelegramBot.Domain.Errors.Service.User;
using DatingTelegramBot.Service.Helpers;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;

namespace DatingTelegramBot.Service.Services.Commands;

public sealed class SearchInterestCommandService(IProfileRequestBuilder profileBuilder,
    ILogger<SearchInterestCommandService> logger) : ICommandService<GenderSearch>
{
    public async Task<Result<GenderSearch, Error>> RetrieveMessageAsync(Update update, string lng)
    {
        string interestMessage = update.Message.Text ?? string.Empty;
        logger.LogInformation("Received interest message from ChatId: {ChatId}. Message: {Message}", update.Message.Chat.Id, interestMessage);

        if (!await IsValidInterestMessage(interestMessage, lng))
        {
            logger.LogWarning("Invalid interest message from ChatId: {ChatId}. Message: {Message}", update.Message.Chat.Id, interestMessage);
            return UserRegistrationErrors.GenderSearchInterestError;
        }

        var genderSearch = await ParseGenderSearch(interestMessage, lng);
        logger.LogInformation("Parsed gender search for ChatId: {ChatId}. Result: {GenderSearch}", update.Message.Chat.Id, genderSearch);

        return genderSearch;
    }

    private async Task<GenderSearch> ParseGenderSearch(string interestMessage, string lng)
    {
        if (interestMessage.Contains(await TranslatorCommandHelper.GetTranslationAsync(lng, "girls_option")))
        {
            return GenderSearch.Woman;
        }
        else if (interestMessage.Contains(await TranslatorCommandHelper.GetTranslationAsync(lng, "guys_option")))
        {
            return GenderSearch.Man;
        }
        else
        {
            return GenderSearch.Anyway;
        }
    }

    private async Task<bool> IsValidInterestMessage(string interestMessage, string lng)
    {
        bool isValid = interestMessage.Contains(await TranslatorCommandHelper.GetTranslationAsync(lng, "girls_option")) ||
                       interestMessage.Contains(await TranslatorCommandHelper.GetTranslationAsync(lng, "guys_option")) ||
                       interestMessage.Contains(await TranslatorCommandHelper.GetTranslationAsync(lng, "no_preference"));

        logger.LogInformation("Interest message validation for ChatId: {ChatId}. Message: {Message}, Valid: {IsValid}",
            interestMessage, isValid);
        return isValid;
    }

    public async Task SendInvalidMessageAsync(Update update, string lng)
    {
        await profileBuilder.SetInvalidOptionMessage(update.Message.Chat.Id, lng).SendAsync();
        logger.LogInformation("Sent invalid option message for ChatId: {ChatId}", update.Message.Chat.Id);
    }

    public async Task SendMessageAsync(Update update, string lng)
    {
        await profileBuilder.SetInteresGenderRequest(update.Message.Chat.Id, lng).SendAsync();
        logger.LogInformation("Sent interest gender request for ChatId: {ChatId}", update.Message.Chat.Id);
    }
}
