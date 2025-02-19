using DatingTelegramBot.Application.Services.Abstractions.Commands;
using DatingTelegramBot.Service.Services.Commands;
using DatingTelegramBot.Service.Services.Telegram.Commands.Base;
using MediatR;
using Microsoft.Extensions.Logging;
using DatingTelegramBot.Application.Repository.Commands;
using DatingTelegramBot.Domain.Enum;

namespace DatingTelegramBot.Service.Services.Telegram.Commands.Profile.Registrations;

public sealed class HandleAgeCommand(ICommandService<Age> ageCommandService,
    ICommandService<Gender> genderCommandService,
    IMediator mediatR,
    ILogger<HandleAgeCommand> logger) : TelegramCommand(mediatR)
{

    public override string Name => "register_handle_age";

    public async override Task ExecuteAsync(global::Telegram.Bot.Types.Update update, string lng)
    {
        logger.LogInformation("Executing HandleAgeCommand for chat ID: {ChatId}", update.Message.Chat.Id);

        var result = await ageCommandService.RetrieveMessageAsync(update, lng);
        if (result._error != null)
        {
            logger.LogWarning("Invalid age message received from chat ID: {ChatId}", update.Message.Chat.Id);
            await ageCommandService.SendInvalidMessageAsync(update, lng);
            return;
        }

        logger.LogInformation("Valid age processed: {Age} for chat ID: {ChatId}", result._value, update.Message.Chat.Id);

        await mediatR.Send(new AddAgeInHashSetCommand(update.Message.Chat.Id,
            result._value.ToString()));

        await genderCommandService.SendMessageAsync(update, lng);
    }
}
