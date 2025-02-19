using DatingTelegramBot.Application.Repository.Commands;
using DatingTelegramBot.Application.Services.Abstractions.Redis;
using MediatR;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace DatingTelegramBot.Application.Repository.Handlers;

public sealed class AddPathPhotoInHashSetCommandHandler(IRedisDbProvider redisDbProvider,
    ILogger<AddPathPhotoInHashSetCommandHandler> logger)
    : IRequestHandler<AddPathPhotoInHashSetCommand>
{
    public async Task Handle(AddPathPhotoInHashSetCommand request, CancellationToken cancellationToken)
    {
        var registerForm = await redisDbProvider.Database.HashGetAllAsync(request.ChatId.ToString());
        var registerForm2 = registerForm.Select(x => new HashEntry(x.Name, x.Value)).ToList();

        registerForm2.Add(new HashEntry("photo", request.PhotoPath));

        logger.LogInformation("Adding photo path to hash set for ChatId: {ChatId}. Photo Path: {PhotoPath}",
           request.ChatId,
           request.PhotoPath);

        await redisDbProvider.Database.HashSetAsync(request.ChatId.ToString(), registerForm2.ToArray());
        TimeSpan expiration = TimeSpan.FromHours(1);
        await redisDbProvider.Database.KeyExpireAsync(request.ChatId.ToString(), expiration);
    }
}
