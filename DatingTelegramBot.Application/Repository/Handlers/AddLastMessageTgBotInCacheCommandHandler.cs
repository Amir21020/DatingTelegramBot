using DatingTelegramBot.Application.Repository.Commands;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace DatingTelegramBot.Application.Repository.Handlers;

public sealed class AddLastMessageTgBotInCacheCommandHandler(IMemoryCache cacheRepository,
    ILogger<AddLastMessageTgBotInCacheCommandHandler> logger)
    : IRequestHandler<AddLastMessageTgBotInCacheCommand>
{
    public Task Handle(AddLastMessageTgBotInCacheCommand request, CancellationToken cancellationToken)
    {
        var cacheKey = GenerateCacheKey(request.ChatId);

        cacheRepository.Set(cacheKey, request.LastTextTgBot);

        return Task.CompletedTask;
    }

    private string GenerateCacheKey(long chatId)
    {
        return $"lastTextTgBot_{chatId}";
    }
}
