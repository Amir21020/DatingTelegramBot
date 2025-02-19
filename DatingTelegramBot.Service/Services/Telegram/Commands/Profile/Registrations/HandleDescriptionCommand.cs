using DatingTelegramBot.Application.Repository.Commands;
using DatingTelegramBot.Application.Services.Abstractions.Commands;
using DatingTelegramBot.Domain.Errors;
using DatingTelegramBot.Service.Services.Commands;
using DatingTelegramBot.Service.Services.Telegram.Commands.Base;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DatingTelegramBot.Service.Services.Telegram.Commands.Profile.Registrations;

public sealed class HandleDescriptionCommand(IMediator mediatR,
    ICommandService<Description> descriptionCommandService,
    ICommandService<Photo> photoCommandService,
    ILogger<HandleDescriptionCommand> logger) : TelegramCommand(mediatR)
{

    public override string Name => "register_handle_descriprion";

    public async override Task ExecuteAsync(global::Telegram.Bot.Types.Update update, string lng)
    {
        logger.LogInformation("Executing command {CommandName} with lng: {Lng} for chat ID: {ChatId}",
            Name, lng, update.Message.Chat.Id);

        var descriptionResult = await GetDescriptionAsync(update, lng);
        if (descriptionResult is null || descriptionResult._value is not null)
            await AddDescriptionAndSendPhotoAsync(update, descriptionResult == null ? null : descriptionResult._value.ToString(), lng);
        return;
    }

    private async Task<Result<Description, Error>> GetDescriptionAsync(global::Telegram.Bot.Types.Update update, string lng)
    {
        var descriptionResult = await descriptionCommandService.RetrieveMessageAsync(update, lng);

        if (descriptionResult is null || descriptionResult._value is not null)
        {
            return descriptionResult;
        }
        else
        {
            await HandleError(descriptionResult, update, lng);
            return descriptionResult;
        }
    }

    private async Task AddDescriptionAndSendPhotoAsync(global::Telegram.Bot.Types.Update update, string description, string lng)
    {
        await mediatR.Send(new AddDescriptionInHashSetCommand(update.Message.Chat.Id, description));
        await photoCommandService.SendMessageAsync(update, lng);
    }

    private async Task HandleError(Result<Description, Error> descriptionResult, global::Telegram.Bot.Types.Update update, string lng)
    {
        logger.LogError("Error occurred while retrieving descript6ion for chat ID: {ChatId}. Error details: {@Error}",
            update.Message.Chat.Id, descriptionResult._error);
        await descriptionCommandService.SendInvalidMessageAsync(update, lng);
    }


}
