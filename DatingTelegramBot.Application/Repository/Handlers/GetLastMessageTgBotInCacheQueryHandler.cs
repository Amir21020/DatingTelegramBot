using DatingTelegramBot.Application.Repository.Query;
using DatingTelegramBot.Domain.Errors;
using DatingTelegramBot.Domain.Errors.Repository;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace DatingTelegramBot.Application.Repository.Handlers;

public sealed class GetLastMessageTgBotInCacheQueryHandler(IMemoryCache cacheRepository,
    ILogger<GetLastMessageTgBotInCacheQueryHandler> logger)
    : IRequestHandler<GetLastMessageTgBotInCacheQuery, Result<string, Error>>
{
    public async Task<Result<string, Error>> Handle(GetLastMessageTgBotInCacheQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = GenerateCacheKey(request.ChatId);

        if (cacheRepository.TryGetValue(cacheKey, out string lastCommand))
        {
            logger.LogInformation("Successfully retrieved last command from cache with key: {CacheKey} for chat ID: {ChatId}", cacheKey, request.ChatId);
            return lastCommand;
        }

        logger.LogWarning("Last command not found in cache for chat ID: {ChatId}.", request.ChatId);
        return CacheNotFoundErrors.LastCommandIsNotFoundError;
    }

    private string GenerateCacheKey(long chatId)
    {
        return $"lastTextTgBot_{chatId}";
    }
}
