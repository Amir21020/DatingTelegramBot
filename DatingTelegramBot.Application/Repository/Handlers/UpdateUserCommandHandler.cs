using DatingTelegramBot.Application.Repository.Abstraction;
using DatingTelegramBot.Application.Repository.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DatingTelegramBot.Application.Repository.Handlers;

public sealed class UpdateUserCommandHandler(IDatingDbContext dbContext,
    ILogger<UpdateUserCommandHandler> logger) :
     IRequestHandler
    <UpdateUserCommand>
{
    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating user. ChatId: {ChatId}, NewUserName: {@UserName}", request.ChatId, request.UserName);

        var user = await dbContext.Users.FirstOrDefaultAsync(x => x.TgChatId == request.ChatId, cancellationToken);

        if (user != null)
        {
            logger.LogInformation("User found. Updating details for UserId: {UserId}", user.TgChatId);

            user.UserName = request.UserName;
            user.Age = request.Age;
            user.ImagePath = request.ImagePath;
            user.Coordinates = request.Coordinates;
            user.Gender = request.Gender;
            user.SearchInterest = request.SearchInterest;
            user.Description = request.Description;

            await dbContext.SaveChangesAsync();
            logger.LogInformation("User updated successfully. UserId: {UserId}", user.TgChatId);
        }
        else
        {
            logger.LogWarning("User not found for ChatId: {ChatId}. No update performed.", request.ChatId);
        }
    }
}
