using DatingTelegramBot.Application.Repository.Commands;
using DatingTelegramBot.Application.Repository.Options;
using DatingTelegramBot.Application.Services.Abstractions.Redis;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace DatingTelegramBot.Application.Repository.Handlers;

public sealed class SetLanguageCommandHandler(IRedisDbProvider redisDbProvider,
    IOptions<CachePrefixOptions> cachePrefixOptions,
    ILogger<SetLanguageCommandHandler> logger) : IRequestHandler<SetLanguageCommand>
{
    private readonly IDatabase redis = redisDbProvider.Database;
    public async Task Handle(SetLanguageCommand request, CancellationToken cancellationToken)
    {
        string cacheKey = GenerateCacheKey(request.ChatId);
        logger.LogInformation("Setting language in cache. Key: {CacheKey}, Language: {@Language}", cacheKey, request.Language);

        await redis?.StringSetAsync(cacheKey, request.Language.ToString());

        logger.LogInformation("Successfully set language in cache for key: {CacheKey}", cacheKey);

        await Task.CompletedTask;
    }

    private string GenerateCacheKey(long chatId)
    {
        return $"{cachePrefixOptions.Value.LanguagePrefix}_{chatId}";
    }
}
