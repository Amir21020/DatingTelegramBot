using DatingTelegramBot.Application.Services.Abstractions.Commands;
using DatingTelegramBot.Service.Services.Commands;
using DatingTelegramBot.Service.Services.Telegram.Commands.Base;
using MediatR;

namespace DatingTelegramBot.Service.Services.Telegram.Commands.Profile.Updates;

public sealed class SendUpdatePhotoCommand(IMediator mediatR,
    IUpdateCommandService<Photo> photoUpdateCommandService)
    : TelegramCommand(mediatR)
{
    public override string Name => "send_update_photo";

    public override async Task ExecuteAsync(global::Telegram.Bot.Types.Update update, string lng)
    {
        await photoUpdateCommandService.SendMessageAsync(update,
            lng);
    }
}
