using DatingTelegramBot.Domain.DTO.Request;
using DatingTelegramBot.Domain.Errors;
using Telegram.Bot.Types;

namespace DatingTelegramBot.Application.Services.Abstractions.Commands;

public interface IProfileViewCommandService
{
    Task SendProfileAsync(SendProfileRequest request);
    Task<Result<SendProfileRequest, Error>> GetViewUserProfileAsync(Update update, int profileIndex, string lng);
    Task HandleUserProfileRequestAsync(Update update, string lng);
    Task SendProfileByEstimationAsync(SendProfileRequest request);
}
