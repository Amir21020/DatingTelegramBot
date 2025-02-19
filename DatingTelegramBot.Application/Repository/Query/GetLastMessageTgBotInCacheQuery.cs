using DatingTelegramBot.Domain.Errors;
using MediatR;

namespace DatingTelegramBot.Application.Repository.Query;

public sealed record GetLastMessageTgBotInCacheQuery(long ChatId) : IRequest<Result<string, Error>>;
