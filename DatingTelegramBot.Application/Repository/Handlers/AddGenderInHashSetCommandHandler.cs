using DatingTelegramBot.Application.Repository.Commands;
using DatingTelegramBot.Application.Services.Abstractions.Redis;
using MediatR;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace DatingTelegramBot.Application.Repository.Handlers;

public sealed class AddGenderInHashSetCommandHandler(IRedisDbProvider redisDbProvider, ILogger<AddGenderInHashSetCommandHandler> logger)
    : IRequestHandler<AddGenderInHashSetCommand>
{
    public async Task Handle(AddGenderInHashSetCommand request, CancellationToken cancellationToken)
    {
        var registerForm = await redisDbProvider.Database.HashGetAllAsync(request.ChatId.ToString());
        var registerForm2 = registerForm.Select(x => new HashEntry(x.Name, x.Value)).ToList();
        registerForm2.Add(new HashEntry("gender", request.Gender));
        logger.LogInformation("Adding gender to hash set for ChatId: {ChatId}. Gender: {Gender}",
            request.ChatId,
            request.Gender);
        await redisDbProvider.Database.HashSetAsync(request.ChatId.ToString(),
            registerForm2.ToArray());
    }
}
