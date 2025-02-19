using Telegram.Bot.Types;

namespace DatingTelegramBot.Application.Services.Abstractions.Commands;

public interface IStickerCommandService
{
    Task HandleDislikeAsync(Update update, string lng);
    Task HandleLikeAsync(int currentIndex, Update update, string lng);
}
