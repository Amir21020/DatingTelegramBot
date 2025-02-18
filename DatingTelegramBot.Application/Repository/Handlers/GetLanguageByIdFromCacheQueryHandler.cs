using DatingTelegramBot.Application.Repository.Options;
using DatingTelegramBot.Application.Repository.Query;
using DatingTelegramBot.Application.Services.Abstractions.Redis;
using DatingTelegramBot.Domain.Enum;
using DatingTelegramBot.Domain.Errors;
using DatingTelegramBot.Domain.Errors.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace DatingTelegramBot.Application.Repository.Handlers;

public sealed class GetLanguageByIdFromCacheQueryHandler
    (IRedisDbProvider redisDbProvider,
    IOptions<CachePrefixOptions> cachePrefixOptions,
    ILogger<GetLanguageByIdFromCacheQueryHandler> logger) : IRequestHandler<GetLanguageByIdFromCacheQuery,
    Result<Language,Error>>
{
    private readonly IDatabase redis = redisDbProvider.Database;
    public async Task<Result<Language, Error>> Handle(GetLanguageByIdFromCacheQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = GenerateCacheKey(request.Id);
        logger.LogInformation("Looking for language in cache with key: {CacheKey}", cacheKey);

        var lng = (string) await redis.StringGetAsync(cacheKey);


        if (lng is null)
        {
            logger.LogWarning("Language not found in cache for key: {CacheKey}", cacheKey);
            return CacheNotFoundErrors.LanguageIsNotFoundError;
        }
        var lngResult =  Enum.Parse<Language>(lng);
        logger.LogInformation("Language found in cache for key: {CacheKey}. Language: {@Language}", cacheKey, lngResult);
        return lngResult;
    }
    private string GenerateCacheKey(long userId)
    {
        return $"{cachePrefixOptions.Value.LanguagePrefix}_{userId}";
    }
}
