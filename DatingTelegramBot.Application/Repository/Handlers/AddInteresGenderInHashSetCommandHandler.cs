using DatingTelegramBot.Application.Repository.Commands;
using DatingTelegramBot.Application.Services.Abstractions.Redis;
using MediatR;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace DatingTelegramBot.Application.Repository.Handlers;

public sealed class AddInteresGenderInHashSetCommandHandler(IRedisDbProvider redisDbProvider,
    ILogger<AddInteresGenderInHashSetCommandHandler> logger)
    : IRequestHandler<AddInteresGenderInHashSetCommand>
{
    public async Task Handle(AddInteresGenderInHashSetCommand request, CancellationToken cancellationToken)
    {
        var registerForm = await redisDbProvider.Database.HashGetAllAsync(request.ChatId.ToString());
        var registerForm2 = registerForm.Select(x => new HashEntry(x.Name, x.Value)).ToList();
        registerForm2.Add(new HashEntry("interes_gender", request.InteresGender.ToString()));
        logger.LogInformation("Adding interested gender to hash set for ChatId: {ChatId}. Interested Gender: {InterestedGender}",
            request.ChatId,
            request.InteresGender.ToString());

        await redisDbProvider.Database.HashSetAsync(request.ChatId.ToString(),
            registerForm2.ToArray());
    }
}
