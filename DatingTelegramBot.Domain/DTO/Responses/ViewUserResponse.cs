using DatingTelegramBot.Domain.Entity;
using DatingTelegramBot.Domain.Enum;

namespace DatingTelegramBot.Domain.DTO.Responses;

public sealed record ViewUserResponse(
        string ImagePath,
        string UserName,
        int Age,
        string Description,
        Coordinates Coordinates,
        Gender Gender,
        GenderSearch InterestGender,
        string City);
