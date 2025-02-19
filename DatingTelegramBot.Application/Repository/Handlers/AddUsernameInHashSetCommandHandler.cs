using DatingTelegramBot.Application.Repository.Commands;
using DatingTelegramBot.Application.Services.Abstractions.Redis;
using MediatR;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace DatingTelegramBot.Application.Repository.Handlers;

public sealed class AddUsernameInHashSetCommandHandler
    (IRedisDbProvider redisDbProvider, ILogger<AddUsernameInHashSetCommandHandler> logger): IRequestHandler<AddUsermameInHashSetCommand>
{
    public async Task Handle(AddUsermameInHashSetCommand request, CancellationToken cancellationToken)
    {
        var registerForm = await redisDbProvider.Database.HashGetAllAsync(request.ChatId.ToString());
        var registerForm2 = registerForm.Select(x => new HashEntry(x.Name, x.Value)).ToList();
        registerForm2.Add(new("username", request.UserName));
        logger.LogInformation("Adding username to hash set for ChatId: {ChatId}. Username: {UserName}",
           request.ChatId,
           request.UserName);

        await redisDbProvider.Database.HashSetAsync(request.ChatId.ToString(),
                registerForm2.ToArray());
    }
}
