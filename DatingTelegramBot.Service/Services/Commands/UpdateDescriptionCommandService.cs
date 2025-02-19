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

public sealed class UpdateDescriptionCommandService
    (IDescriptionRequestBuilder descriptionRequestBuilder, ILogger<BaseDescriptionCommandService> logger) : BaseDescriptionCommandService(descriptionRequestBuilder, logger), IUpdateCommandService<Description>
{

    public async Task<Result<Description, Error>> RetrieveMessageAsync(Update update, string lng)
    {
        if (update.Message.Type == MessageType.Text)
        {
            var (chatId, description) = GetChatIdAndDescription(update);

            var goToBackTranslation = await TranslatorCommandHelper.GetTranslationAsync(lng, "key_back");

            if (description.Contains(goToBackTranslation, StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogInformation("Description from ChatId: {ChatId} matches 'key_back'.", chatId);
                return UserUpdateErrors.DescriptionUpdateCancelledError;
            }
            _logger.LogInformation("Valid description received from ChatId: {ChatId}, Description: {Description}.", chatId, description);
            return new Description(description);
        }
        _logger.LogWarning("Invalid message type received from ChatId: {ChatId}.", update.Message.Chat.Id);
        return UserRegistrationErrors.InvalidMessageTypeError;
    }

    public Task SendInvalidMessageAsync(Update update, string lng)
    {
        var chatId = update.Message.Chat.Id;
        _logger.LogWarning("Sending invalid message for ChatId: {ChatId}, Language: {Language}.", chatId, lng);
        return _descriptionRequestBuilder.SetUpdateErrorDescriptionRequest(chatId, lng).SendAsync();
    }

    public async Task SendMessageAsync(Update update, string lng)
    {
        var chatId = update.Message.Chat.Id;
        _logger.LogInformation("Sending update message for ChatId: {ChatId}, Language: {Language}.", chatId, lng);
        await _descriptionRequestBuilder.SetUpdateDescriptionRequest(chatId, lng).SendAsync();
        _logger.LogInformation("Update message sent successfully to ChatId: {ChatId}.", chatId);
    }
}
