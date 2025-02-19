using DatingTelegramBot.Application.Repository.Commands;
using DatingTelegramBot.Application.Services.Abstractions.Commands;
using DatingTelegramBot.Domain.Entity;
using DatingTelegramBot.Domain.Enum;
using DatingTelegramBot.Domain.Errors;
using DatingTelegramBot.Service.Services.Telegram.Commands.Base;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DatingTelegramBot.Service.Services.Telegram.Commands.Profile.Registrations;

public sealed class HandleInteresGenderCommand(
    ICommandService<GenderSearch> genderCommandService,
    IMediator mediatR,
    ICommandService<Coordinates> coordinateCommandService,
    ILogger<HandleInteresGenderCommand> logger)
    : TelegramCommand(mediatR)
{
    public override string Name => "register_handle_interes_gender";

    public override async Task ExecuteAsync(global::Telegram.Bot.Types.Update update, string lng)
    {
        logger.LogInformation("Executing command {CommandName} for chat ID: {ChatId} with language: {Lng}",
            Name,
            update.Message.Chat.Id,
            lng);

        var result = await genderCommandService.RetrieveMessageAsync(update, lng);

        if (result._error is not null)
        {
            await HandleError(result, update, lng);
            return;
        }

        await HandleSuccess(update, result._value, lng);
    }

    private async Task HandleError(Result<GenderSearch, Error> result, global::Telegram.Bot.Types.Update update, string lng)
    {
        logger.LogError("Error occurred while retrieving interest gender for chat ID: {ChatId}. Error details: {@Error}",
            update.Message.Chat.Id,
            result._error);
        await genderCommandService.SendInvalidMessageAsync(update, lng);
    }

    private async Task HandleSuccess(global::Telegram.Bot.Types.Update update, GenderSearch interestGender, string lng)
    {
        logger.LogInformation("Successfully retrieved interest gender for chat ID: {ChatId}. Interest Gender: {InterestGender}",
            update.Message.Chat.Id,
            interestGender);

        await mediatR.Send(new AddInteresGenderInHashSetCommand(update.Message.Chat.Id, interestGender));
        await coordinateCommandService.SendMessageAsync(update, lng);
    }
}