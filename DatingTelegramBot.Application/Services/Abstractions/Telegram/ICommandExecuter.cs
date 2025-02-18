using Telegram.Bot.Types;

namespace DatingTelegramBot.Application.Services.Abstractions.Telegram;

public interface ICommandExecutor
{
    Task ExecuteAsync(Update update);
}
