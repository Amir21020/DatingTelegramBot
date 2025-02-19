using DatingTelegramBot.Application.Repository.Query;
using DatingTelegramBot.Application.Services.Abstractions.Commands;
using DatingTelegramBot.Service.Services.Telegram.Commands.Base;
using MediatR;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;

namespace DatingTelegramBot.Service.Services.Telegram.Commands;

public sealed class HandleLikeCommand(IMediator mediator, IStickerCommandService stickerCommandService,
    ILogger<HandleLikeCommand> logger) : TelegramCommand(mediator)
{
    public override string Name => "handle_like";

    public async override Task ExecuteAsync(Update update, string lng)
    {
        var chatId = update.Message.Chat.Id;

        logger.LogInformation("Executing HandleLikeCommand for ChatId: {ChatId}", chatId);

        var currentIndexResult = await mediatR.Send(new GetCurrentIndexFromCacheQuery(chatId));

        logger.LogInformation("Current index retrieved: {CurrentIndex} for ChatId: {ChatId}", currentIndexResult._value, chatId);

        await stickerCommandService.HandleLikeAsync(currentIndexResult._value, update, lng);
    }
}
