using DatingTelegramBot.Domain.DTO.Responses;
using DatingTelegramBot.Domain.Errors;
using MediatR;

namespace DatingTelegramBot.Application.Repository.Query;

public sealed record GetUserProfilesByLocationAndInterestByIdQuery(long ChatId)
    : IRequest<Result<UserProfileResponse[], Error>>;