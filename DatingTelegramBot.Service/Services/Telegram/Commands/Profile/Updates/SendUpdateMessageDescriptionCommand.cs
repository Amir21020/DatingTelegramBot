using DatingTelegramBot.Application.Services.Abstractions.Commands;
using DatingTelegramBot.Service.Services.Commands;
using DatingTelegramBot.Service.Services.Telegram.Commands.Base;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DatingTelegramBot.Service.Services.Telegram.Commands.Profile;

public sealed class SendMessageDescriptionCommand(IMediator mediator,
        IUpdateCommandService<Description> updateDescriptionCommandService,
        ILogger<SendMessageDescriptionCommand> logger) : TelegramCommand(mediator)
{

    public override string Name => "send_update_message_description";
    public override async Task ExecuteAsync(global::Telegram.Bot.Types.Update update, string lng)
    {
        logger.LogInformation("Executing SendMessageDescriptionCommand for ChatId: {ChatId} with language: {Language}",update.Message.Chat.Id, lng);
        await updateDescriptionCommandService.SendMessageAsync(update, lng);
    }
}
