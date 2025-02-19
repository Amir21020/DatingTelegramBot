using DatingTelegramBot.Application.Helpers;
using DatingTelegramBot.Application.Repository.Query;
using DatingTelegramBot.Application.Services.Abstractions.Redis;
using DatingTelegramBot.Domain.DTO.Responses;
using DatingTelegramBot.Domain.Entity;
using DatingTelegramBot.Domain.Enum;
using DatingTelegramBot.Domain.Errors;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace DatingTelegramBot.Application.Repository.Handlers;

public sealed class GetUserFromHashSetQueryHandler(IRedisDbProvider redisDbProvider, 
    ILogger<GetUserFromHashSetQuery> logger,
    IMapLocationService mapLocationService)
    : IRequestHandler<GetUserFromHashSetQuery, Result<ViewUserResponse, Error>>
{
    public async Task<Result<ViewUserResponse, Error>> Handle(GetUserFromHashSetQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling GetUserFromHashSetQuery for ChatId: {ChatId}", request.ChatId);
        var userHashEntries = await redisDbProvider.Database.HashGetAllAsync(request.ChatId.ToString());
        var imagePath = userHashEntries.FirstOrDefault(x => x.Name == "photo").Value;
        var userName = userHashEntries.FirstOrDefault(h => h.Name == "username").Value;
        var age = int.Parse(userHashEntries.FirstOrDefault(h => h.Name == "age").Value);
        var description = userHashEntries.FirstOrDefault(h => h.Name == "description").Value;

        var gender = ParseGender(userHashEntries.FirstOrDefault(x => x.Name == "gender").Value);
        var interesGender = ParseGenderSearch(userHashEntries.FirstOrDefault(h => h.Name == "interes_gender").Value);

        var longitude = Double.Parse(userHashEntries.FirstOrDefault(h => h.Name == "longitude").Value.ToString().Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture);
        var latitude = Double.Parse(userHashEntries.FirstOrDefault(h => h.Name == "latitude").Value.ToString().Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture);

        var coordinates = new Coordinates(latitude, longitude);
        var city = await mapLocationService.GetCityByCoordinatesAsync(coordinates);

        logger.LogInformation("Successfully retrieved user for ChatId: {ChatId}. UserCity: {City}", request.ChatId, city._value);

        return new ViewUserResponse(imagePath, userName, age, description, coordinates, gender,
           interesGender, city._value);
    }
    private GenderSearch ParseGenderSearch(string genderSearch)
    {
        return genderSearch switch
        {
            "man" => GenderSearch.Man,
            "woman" => GenderSearch.Woman,
            _ => GenderSearch.Anyway,
        };
    }

    private Gender ParseGender(string gender)
    {
        return gender == "man" ? Gender.Man : Gender.Woman;
    }
}
