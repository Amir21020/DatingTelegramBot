using DatingTelegramBot.Application.Repository.Abstraction;
using DatingTelegramBot.Application.Repository.Commands;
using DatingTelegramBot.Application.Repository.Options;
using DatingTelegramBot.Application.Services.Abstractions.Redis;
using DatingTelegramBot.Domain.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System.Text.Json;

namespace DatingTelegramBot.Application.Repository.Handlers;
public sealed class CreateUserCommandHandler(IDatingDbContext dbContext,
     IOptions<CachePrefixOptions> cachePrefixOptions,
    ILogger<CreateUserCommandHandler> logger, IRedisDbProvider redisDbProvider)
    : IRequestHandler<CreateUserCommand>
{
    private readonly IDatabase redis = redisDbProvider.Database;
    public async Task Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling user command for TgChatId: {TgChatId}", request.TgChatId);

        var user = await dbContext.Users.FirstOrDefaultAsync(x => x.TgChatId == request.TgChatId, cancellationToken);

        if (user == null)
        {
            logger.LogInformation("Creating new user with TgChatId: {TgChatId}.", request.TgChatId);
            user = new UserEntity(request.UserName, request.TgUserName, request.Age,
                request.ImagePath, request.Coordinates, request.Gender,
                request.SearchInterest, request.Description, request.TgChatId);
            await dbContext.Users.AddAsync(user);
        }
        else
        {
            logger.LogInformation("Updating existing user with TgChatId: {TgChatId}.", request.TgChatId);
            user.UserName = request.UserName;
            user.TgUserName = request.TgUserName;
            user.Age = request.Age;
            user.ImagePath = request.ImagePath;
            user.Coordinates = request.Coordinates;
            user.Gender = request.Gender;
            user.SearchInterest = request.SearchInterest;
            user.Description = request.Description;
        }
        await dbContext.SaveChangesAsync();

        var userJsonSerializer = JsonSerializer.Serialize<UserEntity>(user);
        TimeSpan ttl = TimeSpan.FromSeconds(60);
        await redis.StringSetAsync(GenerateCacheKey(request.TgChatId), userJsonSerializer, ttl);
     
        logger.LogInformation("User {Action} successfully with TgChatId: {TgChatId}.", user == null ? "created" : "updated", request.TgChatId);
    }
    private string GenerateCacheKey(long chatId)
    {
        return $"{cachePrefixOptions.Value.UserPrefix}_{chatId}";
    }
}