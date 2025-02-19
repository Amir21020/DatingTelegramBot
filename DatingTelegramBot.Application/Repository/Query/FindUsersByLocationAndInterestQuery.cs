using DatingTelegramBot.Domain.DTO.Responses;
using DatingTelegramBot.Domain.Entity;
using DatingTelegramBot.Domain.Enum;
using MediatR;

namespace DatingTelegramBot.Application.Repository.Query;

public sealed record FindUsersByLocationAndInterestQuery(long ChatId, string UserName,
    GenderSearch SearchInterest,
    Coordinates Location, int Age) : IRequest<UserProfileResponse[]>;
