using MediatR;

namespace DatingTelegramBot.Application.Repository.Commands;

public sealed record AddLastMessageTgBotInCacheCommand(long ChatId, string LastTextTgBot) : IRequest;