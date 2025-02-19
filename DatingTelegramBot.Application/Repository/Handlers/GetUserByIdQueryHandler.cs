using DatingTelegramBot.Application.Repository.Abstraction;
using DatingTelegramBot.Application.Repository.Options;
using DatingTelegramBot.Application.Repository.Query;
using DatingTelegramBot.Application.Services.Abstractions.Redis;
using DatingTelegramBot.Domain.DTO.Responses;
using DatingTelegramBot.Domain.Errors;
using DatingTelegramBot.Domain.Errors.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System.Text.Json;

namespace DatingTelegramBot.Application.Repository.Handlers;

public sealed class GetUserByIdQueryHandler(IDatingDbContext dbContext,
    IOptions<CachePrefixOptions> cachePrefixOptions,
    ILogger<GetUserByIdQueryHandler> logger, IRedisDbProvider redisDbProvider) : IRequestHandler<GetUserByIdQuery,
    Result<UserResponse, Error>>
{
    private readonly IDatabase redis = redisDbProvider.Database;
    public async Task<Result<UserResponse, Error>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Request to get user by id: {UserId}", request.Id);

        var userFromCache = await redis.StringGetAsync(GenerateCacheKey(request.Id));

        if (userFromCache.HasValue)
        {
            return JsonSerializer.Deserialize<UserResponse>(userFromCache);
        }


        var user = await dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.TgChatId == request.Id, cancellationToken);

        if (user is null)
        {
            logger.LogWarning("User not found for id: {UserId}", request.Id);
            return EntityNotFoundErrors.UserNotFoundError;
        }

        logger.LogInformation("User found: {UserId}, UserName: {UserName}", user.TgChatId, user.UserName);
        var userResponse = new UserResponse(user.TgChatId, user.UserName,
            user.Age, user.ImagePath, user.Coordinates, user.Gender, user.TgUserName, user.SearchInterest, user.Description);
    
        var userJson = JsonSerializer.Serialize(userResponse);
        await redis.StringSetAsync(GenerateCacheKey(request.Id), userJson);

        return userResponse;
    }
    private string GenerateCacheKey(long chatId)
    {
        return $"{cachePrefixOptions.Value.UserPrefix}_{chatId}";
    }

}
