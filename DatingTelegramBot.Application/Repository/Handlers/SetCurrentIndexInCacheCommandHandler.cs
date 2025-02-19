using DatingTelegramBot.Application.Repository.Commands;
using DatingTelegramBot.Application.Repository.Options;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DatingTelegramBot.Application.Repository.Handlers;

public sealed class SetCurrentIndexInCacheCommandHandler(IMemoryCache memoryCache,
    IOptions<CachePrefixOptions> cachePrefixOptions,
    ILogger<SetCurrentIndexInCacheCommandHandler> logger)
    : IRequestHandler<SetCurrentIndexInCacheCommmand>
{

    public Task Handle(SetCurrentIndexInCacheCommmand request, CancellationToken cancellationToken)
    {
        string cacheKey = GenerateCacheKey(request.ChatId);
        logger.LogInformation("Setting current index in cache. Key: {CacheKey}, CurrentIndex: {CurrentIndex}", cacheKey, request.CurrentIndex);

        memoryCache.Set(cacheKey, request.CurrentIndex);

        logger.LogInformation("Successfully set current index in cache for key: {CacheKey}", cacheKey);

        return Task.CompletedTask;
    }

    private string GenerateCacheKey(long chatId)
    {
        return $"{cachePrefixOptions.Value.CurrentIndexPrefix}_{chatId}";
    }
}