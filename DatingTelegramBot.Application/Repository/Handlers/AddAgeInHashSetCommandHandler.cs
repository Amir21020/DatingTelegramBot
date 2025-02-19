using DatingTelegramBot.Application.Repository.Commands;
using DatingTelegramBot.Application.Services.Abstractions.Redis;
using MediatR;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace DatingTelegramBot.Application.Repository.Handlers;

public sealed class AddAgeInHashSetCommandHandler
    (IRedisDbProvider redisDbProvider,
    ILogger<AddAgeInHashSetCommandHandler> logger)
    : IRequestHandler<AddAgeInHashSetCommand>
{
    private readonly IDatabase redis = redisDbProvider.Database;
    public async Task Handle(AddAgeInHashSetCommand request, CancellationToken cancellationToken)
    {
        var registerForm = new HashEntry[1];
        registerForm[0] = new HashEntry("age", request.Age);
        logger.LogInformation("Attempting to add age '{Age}' for chat ID: {ChatId}", request.Age, request.ChatId);
        await redis
            .HashSetAsync(request.ChatId.ToString(),
            registerForm);
    }
}
