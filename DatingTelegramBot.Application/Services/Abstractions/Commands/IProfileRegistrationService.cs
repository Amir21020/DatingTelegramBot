using Telegram.Bot.Types;

namespace DatingTelegramBot.Application.Services.Abstractions.Commands;

public interface IProfileRegistrationService
{
    Task RegistrationProfileAsync(Update update, string lng);
}
