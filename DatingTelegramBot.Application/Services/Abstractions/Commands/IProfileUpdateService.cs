using Telegram.Bot.Types;

namespace DatingTelegramBot.Application.Services.Abstractions.Commands;

public interface IProfileUpdateService
{
    Task UpdateMediaProfileAsync(Update update, string lng);
    Task UpdateDescriptionProfileAsync(Update update, string lng);
}
