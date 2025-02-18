using Telegram.Bot;

namespace DatingTelegramBot.Domain.Abstractions;

public interface IBot
{
    public Task<TelegramBotClient> GetTelegramBot();
}
