using DatingTelegramBot.Application.Repository.Commands;
using DatingTelegramBot.Application.Services.Abstractions.Redis;
using MediatR;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;


namespace DatingTelegramBot.Application.Repository.Handlers;

public sealed class AddDescriptionInHashSetCommandHandler(IRedisDbProvider redisDbProvider,
    ILogger<AddDescriptionInHashSetCommandHandler> logger)
    : IRequestHandler<AddDescriptionInHashSetCommand>
{
    public async Task Handle(AddDescriptionInHashSetCommand request, CancellationToken cancellationToken)
    {
        var registerForm = await redisDbProvider.Database.HashGetAllAsync(request.ChatId.ToString());
        var registerForm2 = registerForm.Select(x => new HashEntry(x.Name, x.Value)).ToList();
        registerForm2.Add(new HashEntry("description", request.Description));
        logger.LogInformation("Adding description to hash set for ChatId: {ChatId}. Description: {Description}",
           request.ChatId,
           request.Description);
        await redisDbProvider.Database.HashSetAsync(request.ChatId.ToString(), registerForm2.ToArray()); 
    }
}
