namespace DatingTelegramBot.Domain.DTO.Responses;

public sealed record UserProfileResponse(
        long ChatId,
        string UserName,
        int Age,
        string ImagePath,
        string Description,
        string TgUserName,
        string City,
        double Distance);