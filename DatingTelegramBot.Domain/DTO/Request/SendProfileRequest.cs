namespace DatingTelegramBot.Domain.DTO.Request;

public sealed record SendProfileRequest(long ChatId, string? FilePath, string? UserName, int Age, string? MessageCity, string? Description, string? Language);
