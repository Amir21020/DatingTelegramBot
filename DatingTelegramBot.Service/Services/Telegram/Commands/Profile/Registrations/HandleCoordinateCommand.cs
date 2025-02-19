using DatingTelegramBot.Application.Repository.Commands;
using DatingTelegramBot.Application.Services.Abstractions.Commands;
using DatingTelegramBot.Domain.Entity;
using DatingTelegramBot.Domain.Errors;
using DatingTelegramBot.Service.Services.Commands;
using DatingTelegramBot.Service.Services.Telegram.Commands.Base;
using MediatR;
using Microsoft.Extensions.Logging;


namespace DatingTelegramBot.Service.Services.Telegram.Commands.Profile.Registrations;

public sealed class HandleCoordinateCommand(ICommandService<Coordinates> coordinateCommandService,
    IMediator mediatR,
    ICommandService<UserName> userCommandService,
    ILogger<HandleCoordinateCommand> logger)
    : TelegramCommand(mediatR)
{
    public override string Name => "register_handle_coordinate";

    public override async Task ExecuteAsync(global::Telegram.Bot.Types.Update update, string lng)
    {
        logger.LogInformation("Executing command {CommandName} for chat ID: {ChatId} with language: {Lng} and update: {@Update}",
            Name,
            update.Message.Chat.Id,
            lng,
            update);

        var result = await coordinateCommandService.RetrieveMessageAsync(update, lng);


        if (result._error is not null)
        {
            await HandleError(result, update, lng);
            return;
        }

        await HandleSuccess(update, result._value, lng);
    }

    private async Task HandleError(Result<Coordinates, Error> result, global::Telegram.Bot.Types.Update update, string lng)
    {
        logger.LogError("Error occurred while retrieving coordinates for chat ID: {ChatId}. Error details: {@Error}",
            update.Message.Chat.Id,
            result._error);
        await coordinateCommandService.SendInvalidMessageAsync(update, lng);
    }

    private async Task HandleSuccess(global::Telegram.Bot.Types.Update update, Coordinates coordinates, string lng)
    {
        logger.LogInformation("Successfully retrieved coordinates for chat ID: {ChatId}. Latitude: {Latitude}, Longitude: {Longitude}",
            update.Message.Chat.Id,
            coordinates.Latitude,
            coordinates.Longitude);

        await mediatR.Send(new AddCoordinatesInHashSetCommand(update.Message.Chat.Id,
            coordinates.Latitude.ToString(), coordinates.Longitude.ToString()));
        await userCommandService.SendMessageAsync(update, lng);
    }
}
