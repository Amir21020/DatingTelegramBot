using DatingTelegramBot.Application.Repository.Commands;
using DatingTelegramBot.Application.Services.Abstractions.Commands;
using DatingTelegramBot.Domain.Errors;
using DatingTelegramBot.Service.Services.Commands;
using DatingTelegramBot.Service.Services.Telegram.Commands.Base;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DatingTelegramBot.Service.Services.Telegram.Commands.Profile.Registrations;

public sealed class HandleUserNameCommand(IMediator mediatR,
    ICommandService<UserName> userNameCommandService,
    ILogger<HandleUserNameCommand> logger,
    ICommandService<Description> descriptionCommandService) : TelegramCommand(mediatR)
{
    public override string Name => "register_handle_username";
    public async override Task ExecuteAsync(global::Telegram.Bot.Types.Update update, string lng)
    {
        long chatId = update.Message.Chat.Id;
        logger.LogInformation("Initiating username retrieval for chat ID: {ChatId} with language: {Lng}", chatId, lng);

        var userNameResult = await userNameCommandService.RetrieveMessageAsync(update, lng);

        if (userNameResult._error is not null)
        {
            await HandleUserNameRetrievalError(userNameResult, update, lng);
            return;
        }

        await HandleUserNameRetrieved(update, userNameResult._value.ToString(), lng);
    }

    private async Task HandleUserNameRetrievalError(Result<UserName, Error> userNameResult, global::Telegram.Bot.Types.Update update, string lng)
    {
        logger.LogError("Error occurred while retrieving username for chat ID: {ChatId}. Error details: {@Error}",
                         update.Message.Chat.Id,
                         userNameResult._error);
        await userNameCommandService.SendInvalidMessageAsync(update, lng);
    }

    private async Task HandleUserNameRetrieved(global::Telegram.Bot.Types.Update update, string userName, string lng)
    {
        logger.LogInformation("Successfully retrieved username: {UserName} for chat ID: {ChatId}", userName, update.Message.Chat.Id);

        await mediatR.Send(new AddUsermameInHashSetCommand(update.Message.Chat.Id, userName));

        logger.LogInformation("Executing SendMessageDescriptionCommand for chat ID: {ChatId} with language: {Lng}", update.Message.Chat.Id, lng);
        await descriptionCommandService.SendMessageAsync(update, lng);
    }
}
