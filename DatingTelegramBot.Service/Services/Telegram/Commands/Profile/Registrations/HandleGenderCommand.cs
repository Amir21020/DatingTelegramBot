using DatingTelegramBot.Application.Repository.Commands;
using DatingTelegramBot.Application.Services.Abstractions.Commands;
using DatingTelegramBot.Domain.Enum;
using DatingTelegramBot.Domain.Errors;
using DatingTelegramBot.Service.Services.Telegram.Commands.Base;
using MediatR;
using Microsoft.Extensions.Logging;


namespace DatingTelegramBot.Service.Services.Telegram.Commands.Profile.Registrations;

public sealed class HandleGenderCommand(
    ICommandService<Gender> genderCommandService,
    IMediator mediatR,
    ICommandService<GenderSearch> genderSearchService,
    ILogger<HandleGenderCommand> logger) : TelegramCommand(mediatR)
{
    public override string Name => "register_handle_gender";

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

    private async Task HandleError(Result<Gender, Error> result, global::Telegram.Bot.Types.Update update, string lng)
    {
        logger.LogError("Error occurred while retrieving gender for chat ID: {ChatId}. Error details: {@Error}",
            update.Message.Chat.Id,
            result._error);
        await genderCommandService.SendInvalidMessageAsync(update, lng);
    }

    private async Task HandleSuccess(global::Telegram.Bot.Types.Update update, Gender gender, string lng)
    {
        logger.LogInformation("Successfully retrieved gender for chat ID: {ChatId}. Gender: {Gender}",
            update.Message.Chat.Id,
            gender.ToString());

        await mediatR.Send(new AddGenderInHashSetCommand(update.Message.Chat.Id, gender.ToString()));
        await genderSearchService.SendMessageAsync(update, lng);
    }
}
