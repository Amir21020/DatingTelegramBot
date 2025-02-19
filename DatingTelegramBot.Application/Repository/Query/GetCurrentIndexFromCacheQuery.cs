using DatingTelegramBot.Domain.Errors;
using MediatR;

namespace DatingTelegramBot.Application.Repository.Query;

public sealed record GetCurrentIndexFromCacheQuery(long ChatId)
    : IRequest<Result<int, Error>>;