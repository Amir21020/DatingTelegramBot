using DatingTelegramBot.Application.Services.Abstractions.Builder;
using DatingTelegramBot.Application.Services.Abstractions.Commands;
using DatingTelegramBot.Domain.Errors;
using DatingTelegramBot.Domain.Errors.Service.User;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace DatingTelegramBot.Service.Services.Commands;

public sealed class UserNameCommandService(IProfileRequestBuilder profileRequestBuilder,
    ILogger<UserNameCommandService> logger) : ICommandService<UserName>
{
    public async Task<Result<UserName, Error>> RetrieveMessageAsync(Update update, string lng)
    {
        if(update.Message.Type == MessageType.Text)
        {
            var userName = update.Message.Text;

            if (string.IsNullOrWhiteSpace(userName))
            {
                logger.LogWarning("User name was not provided for ChatId: {ChatId}.", update.Message.Chat.Id);
                return UserRegistrationErrors.UserNameIsNotFoundError;
            }

            logger.LogInformation("User name retrieved: {UserName} for ChatId: {ChatId}.", userName, update.Message.Chat.Id);
            return new UserName(userName);
        }

        return UserRegistrationErrors.InvalidMessageTypeError;
    }

    public async Task SendInvalidMessageAsync(Update update, string lng)
    {
        await profileRequestBuilder.SetInvalidNameMessage(update.Message.Chat.Id, update.Message.Chat.FirstName, lng).SendAsync();
        logger.LogInformation("Sent invalid name message for ChatId: {ChatId}", update.Message.Chat.Id);
    }

    public async Task SendMessageAsync(Update update, string lng)
    {
        await profileRequestBuilder.SetNameRequest(update.Message.Chat.Id, update.Message.Chat.FirstName, lng).SendAsync();
        logger.LogInformation("Sent name request message for ChatId: {ChatId}", update.Message.Chat.Id);
    }
}

public sealed class UserName(string fullName)
{
    public override string ToString()
    {
        return fullName;
    }
}