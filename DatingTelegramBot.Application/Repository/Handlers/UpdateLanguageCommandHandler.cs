using DatingTelegramBot.Application.Repository.Commands;
using DatingTelegramBot.Application.Repository.Options;
using DatingTelegramBot.Application.Services.Abstractions.Redis;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace DatingTelegramBot.Application.Repository.Handlers;

public sealed class UpdateLanguageCommandHandler(IRedisDbProvider redisDbProvider,
    IOptions<CachePrefixOptions> cachePrefixOptions,
    ILogger<UpdateLanguageCommandHandler> logger) : IRequestHandler<UpdateLanguageCommand>
{
    private readonly IDatabase redis = redisDbProvider.Database;
    public async Task Handle(UpdateLanguageCommand request, CancellationToken cancellationToken)
    {
        string cacheKey = GenerateCacheKey(request.ChatId);

        logger.LogInformation("Updating language in cache. Key: {CacheKey}, NewLanguage: {@NewLanguage}", cacheKey, request.Lng);

        await redis.StringSetAsync(cacheKey, request.Lng.ToString());
    }

    private string GenerateCacheKey(long chatId)
    {
        return $"{cachePrefixOptions.Value.LanguagePrefix}_{chatId}"; 
    }
}

