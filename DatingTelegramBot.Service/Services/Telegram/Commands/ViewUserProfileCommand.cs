using DatingTelegramBot.Application.Services.Abstractions.Commands;
using DatingTelegramBot.Service.Services.Telegram.Commands.Base;
using MediatR;
using Telegram.Bot.Types;

namespace DatingTelegramBot.Service.Services.Telegram.Commands;

public sealed class ViewUserProfileCommand(IStickerCommandService stickerCommandService, IMediator mediator)
    : TelegramCommand(mediator)
{
    public override string Name => "view_user_profile";

    public async override Task ExecuteAsync(Update update, string lng)
    {
        await stickerCommandService.HandleDislikeAsync(update, lng);
    }
}
