using DatingTelegramBot.Application.Repository.Commands;
using DatingTelegramBot.Application.Services.Abstractions.Redis;
using MediatR;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace DatingTelegramBot.Application.Repository.Handlers;

public sealed class AddCoordinatesInHashSetCommandHandler(IRedisDbProvider redisDbProvider,
    ILogger<AddCoordinatesInHashSetCommandHandler> logger)
    : IRequestHandler<AddCoordinatesInHashSetCommand>
{
    public async Task Handle(AddCoordinatesInHashSetCommand request, CancellationToken cancellationToken)
    {
        var registerForm = await redisDbProvider.Database.HashGetAllAsync(request.ChatId.ToString());
        var registerForm2 = registerForm
        .Select((entry, index) => new HashEntry(entry.Name, entry.Value))
        .ToList();
        registerForm2.Add(new HashEntry("latitude", request.Latitude));
        registerForm2.Add(new HashEntry("longitude", request.Longitude));
        logger.LogInformation("Adding coordinates to hash set for ChatId: {ChatId}. Longitude: {Longitude}, Latitude: {Latitude}",
            request.ChatId,
            request.Longitude,
            request.Latitude);
        await redisDbProvider.Database.HashSetAsync(request.ChatId.ToString(), registerForm2.ToArray());
    }
}
