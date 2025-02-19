using DatingTelegramBot.Application.Services.Abstractions.Commands;
using DatingTelegramBot.Service.Services.Commands;
using DatingTelegramBot.Service.Services.Telegram.Commands.Base;
using MediatR;

namespace DatingTelegramBot.Service.Services.Telegram.Commands.Profile.Registrations;

public sealed class SendAgeCommand(IMediator mediator, ICommandService<Age> ageCommandService) : TelegramCommand(mediator)
{
    public override string Name => "register_send_age";

    public override async Task ExecuteAsync(global::Telegram.Bot.Types.Update update, string lng)
    {
        await ageCommandService.SendMessageAsync(update, lng);
    }
}
