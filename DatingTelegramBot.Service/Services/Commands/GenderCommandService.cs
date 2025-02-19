using DatingTelegramBot.Application.Services.Abstractions.Builder;
using DatingTelegramBot.Application.Services.Abstractions.Commands;
using DatingTelegramBot.Domain.Enum;
using DatingTelegramBot.Domain.Errors;
using DatingTelegramBot.Domain.Errors.Service.User;
using DatingTelegramBot.Service.Helpers;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;

namespace DatingTelegramBot.Service.Services.Commands;

public sealed class GenderCommandService(IProfileRequestBuilder profileBuilder,
    ILogger<GenderCommandService> logger) : ICommandService<Gender>
{
    public async Task<Result<Gender, Error>> RetrieveMessageAsync(Update update, string lng)
    {
        var chatId = update.Message.Chat.Id;
        string genderInput = update.Message.Text.ToString().ToLower();
        logger.LogInformation("Received gender input from ChatId: {ChatId}, Input: {Input}", chatId, genderInput);

        if (await IsValidGender(genderInput, lng, chatId))
        {
            logger.LogWarning("Invalid gender format received from ChatId: {ChatId}, Input: {Input}", chatId, genderInput);
            return UserRegistrationErrors.GenderFormatError;
        }

        var gender = await ParseGender(genderInput, lng, chatId);
        logger.LogInformation("Parsed gender for ChatId: {ChatId}, Gender: {Gender}", chatId, gender);
        return gender;
    }
    private async Task<Gender> ParseGender(string gender, string lng, long chatId)
    {
        var male = await TranslatorCommandHelper.GetTranslationAsync(lng, "male_option");
        return gender == male ? Gender.Man : Gender.Woman;
    }
    private async Task<bool> IsValidGender(string gender, string lng, long chatId)
    {
        var male = await TranslatorCommandHelper.GetTranslationAsync(lng, "male_option");
        var female = await TranslatorCommandHelper.GetTranslationAsync(lng, "female_option");
        bool isValid = gender.Contains(male) || gender.Contains(female);

        logger.LogInformation("Gender validation for ChatId: {ChatId}, Input: {Input}, IsValid: {IsValid}", chatId, gender, isValid);
        return isValid;
    }

    public async Task SendInvalidMessageAsync(Update update, string lng)
    {
        var chatId = update.Message.Chat.Id;
        logger.LogWarning("Sending invalid option message to ChatId: {ChatId}, Language: {Language}", chatId, lng);
        await profileBuilder.SetInvalidOptionMessage(chatId, lng).SendAsync();
        logger.LogInformation("Invalid option message sent successfully to ChatId: {ChatId}", chatId);
    }

    public async Task SendMessageAsync(Update update, string lng)
    {
        var chatId = update.Message.Chat.Id;
        logger.LogInformation("Sending gender request message to ChatId: {ChatId}, Language: {Language}", chatId, lng);
        await profileBuilder.SetGenderRequest(chatId, lng).SendAsync();
        logger.LogInformation("Gender request message sent successfully to ChatId: {ChatId}", chatId);
    }
}
