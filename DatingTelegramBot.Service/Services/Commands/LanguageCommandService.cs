using DatingTelegramBot.Application.Services.Abstractions.Builder;
using DatingTelegramBot.Application.Services.Abstractions.Commands;
using DatingTelegramBot.Domain.Enum;
using DatingTelegramBot.Domain.Errors;
using DatingTelegramBot.Domain.Errors.Service.Language;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;

namespace DatingTelegramBot.Service.Services.Commands;

public sealed class LanguageCommandService
    (ILanguageRequestBuilder languageRequestBuilder, ILogger<LanguageCommandService> logger) : ICommandService<Language>
{
    public async Task<Result<Language, Error>> RetrieveMessageAsync(Update update, string lng)
    {
        var chatId = update.Message.Chat.Id;
        var userMessage = update.Message.Text;
        logger.LogInformation("Received language change request from ChatId: {ChatId}. Message: {Message}", chatId, userMessage);

        if (!CheckLanguageRequest(userMessage))
        {
            logger.LogWarning("Invalid language option provided for ChatId: {ChatId}. Message: {Message}", chatId, userMessage);
            return LanguageCommandErrors.InvalidLanguageError;
        }
        else
        {
            var language = StringParse(userMessage);
            logger.LogInformation("Setting language for ChatId: {ChatId} to {Language}", chatId, language);
            return language;
        }
    }
    private bool CheckLanguageRequest(string message)
    {
        string[] lng = { "русский", "english" };
        bool isValid = lng.Contains(message.ToLower());
        logger.LogInformation("Message: {Message}, IsValid: {IsValid}", message, isValid);
        return isValid;
    }
    private Language StringParse(string text)
    {
        return text switch
        {
            "english" => Language.en,
            _ => Language.ru
        };
    }

    public async Task SendInvalidMessageAsync(Update update, string lng)
    {
        var chatId = update.Message.Chat.Id;

        logger.LogWarning("Invalid language request for ChatId: {ChatId} with Language: {Language}", chatId, lng);

        await languageRequestBuilder.SetInvalidLanguageRequest(chatId, lng)
            .SendAsync();

        logger.LogInformation("Invalid language message sent for ChatId: {ChatId} with Language: {Language}", chatId, lng);
    }

    public async Task SendMessageAsync(Update update, string lng)
    {
        var chatId = update.Message.Chat.Id;
        logger.LogInformation("Executing language change command for ChatId: {ChatId}", chatId);

        await languageRequestBuilder.SetLanguageRequest(chatId, lng).SendAsync();

        logger.LogInformation("Language change request sent for ChatId: {ChatId} with Language: {Language}", chatId, lng);
    }
}