using DatingTelegramBot.Application.Repository.Options;
using DatingTelegramBot.Application.Repository.Query;
using DatingTelegramBot.Domain.Errors;
using DatingTelegramBot.Domain.Errors.Repository;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DatingTelegramBot.Application.Repository.Handlers;

public sealed class GetCurrentIndexFromCacheQueryHandler(
    IMemoryCache memoryCache,
    IOptions<CachePrefixOptions> cachePrefixOptions,
    ILogger<GetCurrentIndexFromCacheQueryHandler> logger)
    : IRequestHandler<GetCurrentIndexFromCacheQuery, Result<int, Error>>
{
    public async Task<Result<int, Error>> Handle(GetCurrentIndexFromCacheQuery request,
        CancellationToken cancellationToken)
    {
        string cacheKey = GenerateCacheKey(request.ChatId);

        logger.LogInformation("Looking for CurrentIndex in cache with key: {CacheKey}", cacheKey);

        if (memoryCache.TryGetValue(cacheKey, out int currentIndex))
        {
            logger.LogInformation("CurrentIndex found in cache: {CurrentIndex}", currentIndex);
            return await Task.FromResult(currentIndex);
        }

        logger.LogWarning("CurrentIndex not found in cache for key: {CacheKey}", cacheKey);
        return await Task.FromResult(CacheNotFoundErrors.CurrentIndexIsNotFoundError);
    }

    private string GenerateCacheKey(long chatId)
    {
        return $"{cachePrefixOptions.Value.CurrentIndexPrefix}_{chatId}";
    }
}
