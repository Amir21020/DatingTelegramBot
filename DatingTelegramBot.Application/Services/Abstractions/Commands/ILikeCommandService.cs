using DatingTelegramBot.Domain.DTO.Responses;

namespace DatingTelegramBot.Application.Services.Abstractions.Commands;

public interface ILikeCommandService
{
    Task HandleLikeAsync(UserProfileResponse likedUser, UserResponse user, string lng);
}
