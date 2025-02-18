using DatingTelegramBot.Domain.Enum;
using DatingTelegramBot.Domain.Errors;
using MediatR;

namespace DatingTelegramBot.Application.Repository.Query;

public sealed record GetLanguageByIdFromCacheQuery(long Id) : IRequest<Result<Language,Error>>;
