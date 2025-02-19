using DatingTelegramBot.Domain.Errors;
using Telegram.Bot.Types;

namespace DatingTelegramBot.Application.Services.Abstractions.Commands;

public interface ICommandService<T>
{
    Task SendMessageAsync(Update update, string lng);
    Task SendInvalidMessageAsync(Update update, string lng);
    Task<Result<T, Error>> RetrieveMessageAsync(Update update, string lng);
}