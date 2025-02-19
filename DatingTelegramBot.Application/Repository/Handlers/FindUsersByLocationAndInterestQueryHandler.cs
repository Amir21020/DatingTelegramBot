using DatingTelegramBot.Application.Helpers;
using DatingTelegramBot.Application.Repository.Abstraction;
using DatingTelegramBot.Application.Repository.Query;
using DatingTelegramBot.Domain.DTO.Responses;
using DatingTelegramBot.Domain.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace DatingTelegramBot.Application.Repository.Handlers;

public sealed class FindUsersByLocationAndInterestQueryHandler(IDatingDbContext dbContext,
    IMapLocationService mapLocationQueryService,
    ILogger<FindUsersByLocationAndInterestQueryHandler> logger)
    : IRequestHandler<FindUsersByLocationAndInterestQuery, UserProfileResponse[]>
{
    public async Task<UserProfileResponse[]> Handle(FindUsersByLocationAndInterestQuery request, CancellationToken cancellationToken)
    {
        var requestLocation = new Coordinates(request.Location.Latitude, request.Location.Longitude);
        logger.LogInformation("Starting to find users by location and interest. Request: {@Request}", request);


        var userResults = await dbContext.Users
            .Where(x => x.TgChatId != request.ChatId)
            .Select(x => new
            {
                User = x,
                Distance = DistanceCalculatorHelper.CalculateDistance(
                    new Coordinates(x.Coordinates.Latitude, x.Coordinates.Longitude),
                    requestLocation)
            })
            .ToListAsync();

        logger.LogInformation("{FoundUsersCount} users found.", userResults.Count);

        var sortedResults = userResults
            .OrderBy(u => u.Distance)
            .ThenBy(x => Math.Abs(x.User.Age - request.Age))
            .ToList();

        var tasks = sortedResults.Select(async u =>
        {
            var city = await mapLocationQueryService.GetCityByCoordinatesAsync(u.User.Coordinates);
            logger.LogInformation("User {UserId} is located in city: {City}", u.User.TgChatId, city._value);
            return new UserProfileResponse(
                u.User.TgChatId,
                u.User.UserName,
                u.User.Age,
                u.User.ImagePath,
                u.User.Description,
                u.User.TgUserName,
                city._value,
                u.Distance);
        });

        var results = await Task.WhenAll(tasks);

        logger.LogInformation("Returning {ResultsCount} user profiles.", results.Length);
        return results;
    }

}