using DatingTelegramBot.Application.Services.Abstractions.Builder;
using DatingTelegramBot.Application.Services.Abstractions.Commands;
using DatingTelegramBot.Domain.Errors;
using DatingTelegramBot.Domain.Errors.Service.User;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;

namespace DatingTelegramBot.Service.Services.Commands;

public sealed class AgeCommandService(IAgeRequestBuilder ageRequestBuilder,
    ILogger<AgeCommandService> logger) : ICommandService<Age>
{
    public async Task SendMessageAsync(Update update, string lng)
    {
        var chatId = update.Message.Chat.Id;
        logger.LogInformation("Sending age request message to ChatId: {ChatId}, Language: {Language}", chatId, lng);

        await ageRequestBuilder
            .SetAgeRequest(chatId, lng)
            .SendAsync();

        logger.LogInformation("Age request message sent successfully to ChatId: {ChatId}", chatId);
    }

    public async Task SendInvalidMessageAsync(Update update, string lng)
    {
        var chatId = update.Message.Chat.Id;
        logger.LogInformation("Sending invalid age confirmation message to ChatId: {ChatId}, Language: {Language}", chatId, lng);

        await ageRequestBuilder
            .SetAgeConfirmationRequest(chatId, lng)
            .SendAsync();            

        logger.LogInformation("Invalid age confirmation message sent successfully to ChatId: {ChatId}", chatId);
    }

    public async Task<Result<Age, Error>> RetrieveMessageAsync(Update update, string lng)
    {
        var chatId = update.Message.Chat.Id;
        string ageInput = update.Message.Text.Trim();

        logger.LogInformation("Received age input from ChatId: {ChatId}, Input: {Input}", chatId, ageInput);

        if (!IsValidAge(ageInput))
        {
            logger.LogWarning("Invalid age format received from ChatId: {ChatId}. Input: {Input}", chatId, ageInput);
            return UserRegistrationErrors.AgeFormatError;
        }

        var age = new Age(int.Parse(ageInput));
        logger.LogInformation("Age successfully parsed from input for ChatId: {ChatId}, Age: {Age}", chatId, ageInput);
        return age;
    }
    private bool IsValidAge(string input)
    {
        bool isValidNumber = int.TryParse(input, out int age);

        bool isValidAgeRange = age >= 0 && age <= 120;

        return isValidNumber && isValidAgeRange;
    }
}


public sealed class Age(int value)
{
    public override string ToString()
    {
        return value.ToString();
    }
}