using DatingTelegramBot.Domain.Entity;
using DatingTelegramBot.Domain.Enum;

namespace DatingTelegramBot.Domain.DTO.Responses;

public sealed record UserResponse(
        long ChatId,
        string UserName,
        int Age,
        string ImagePath,
        Coordinates Coordinates,
        Gender Gender,
        string TgUserName,
        GenderSearch SearchInterest,
        string Description);