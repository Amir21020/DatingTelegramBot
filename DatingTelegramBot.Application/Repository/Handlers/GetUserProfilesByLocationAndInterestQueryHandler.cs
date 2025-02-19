using DatingTelegramBot.Application.Repository.Query;
using DatingTelegramBot.Domain.DTO.Responses;
using DatingTelegramBot.Domain.Errors;
using DatingTelegramBot.Domain.Errors.Service.User;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DatingTelegramBot.Application.Repository.Handlers;

public sealed class GetUserProfilesByLocationAndInterestByIdQueryHandler(
    IMediator mediator,
    ILogger<GetUserProfilesByLocationAndInterestByIdQueryHandler>
    logger)
    : IRequestHandler<GetUserProfilesByLocationAndInterestByIdQuery, Result<UserProfileResponse[], Error>>
{

    public async Task<Result<UserProfileResponse[], Error>> Handle(GetUserProfilesByLocationAndInterestByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Processing request to get user profiles by location and interest for chatId: {ChatId}", request.ChatId);

        var userResult = await GetUserById(request.ChatId);

        if (userResult._error is not null)
        {
            logger.LogWarning("Error occurred while fetching user by ID: {ChatId}. Error: {Error}", request.ChatId, userResult._error);
            return userResult._error;
        }

        var profilesResult = await FindUsersProfiles(userResult._value);

        if (profilesResult._value is null)
        {
            logger.LogWarning("No profiles found for user: {UserId}.", userResult._value.ChatId);
            return UserProfileCommandServiceErrors.NoProfilesFoundError;
        }

        logger.LogInformation("Found {ProfileCount} profiles for user: {UserId}.", profilesResult._value.Length, userResult._value.ChatId);
        return profilesResult;
    }

    private async Task<Result<UserResponse, Error>> GetUserById(long chatId)
    {
        return await mediator.Send(new GetUserByIdQuery(chatId));
    }

    private async Task<Result<UserProfileResponse[], Error>> FindUsersProfiles(UserResponse user)
    {
        var usersQuery = new FindUsersByLocationAndInterestQuery(
            user.ChatId,
            user.UserName,
            user.SearchInterest,
            user.Coordinates,
            user.Age);

        return await mediator.Send(usersQuery);
    }
}
