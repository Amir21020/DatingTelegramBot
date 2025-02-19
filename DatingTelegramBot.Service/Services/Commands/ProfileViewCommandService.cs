using DatingTelegramBot.Application.Helpers;
using DatingTelegramBot.Application.Repository.Commands;
using DatingTelegramBot.Application.Repository.Query;
using DatingTelegramBot.Application.Services.Abstractions.Builder;
using DatingTelegramBot.Application.Services.Abstractions.Commands;
using DatingTelegramBot.Domain.DTO.Request;
using DatingTelegramBot.Domain.Errors;
using MediatR;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;
namespace DatingTelegramBot.Service.Services.Commands;

public sealed class ProfileViewCommandService(IMediator mediatR,
    IProfileRequestBuilder profileRequestBuilder,
    IMapLocationService mapLocationService,
    IPhotoHelper photoHelper, ILogger<ProfileViewCommandService> logger) : IProfileViewCommandService
{
    public async Task<Result<SendProfileRequest, Error>> GetViewUserProfileAsync(Update update,
        int profileIndex,
        string lng)
    {
        logger.LogInformation("Retrieving user profile for ChatId: {ChatId}, ProfileIndex: {ProfileIndex}", update.Message.Chat.Id, profileIndex);

        var users = await mediatR.Send(new GetUserProfilesByLocationAndInterestByIdQuery(update.Message.Chat.Id));

        if (users._error is not null)
        {
            logger.LogError("Error retrieving user profiles for ChatId: {ChatId}. Error: {Error}",
                update.Message.Chat.Id,
                users._error);
            return users._error;
        }

        var likedUser = users._value[profileIndex];

        logger.LogInformation("User profile retrieved: UserName: {UserName}, Age: {Age}, City: {City}",
            likedUser.UserName, likedUser.Age, likedUser.City);

        return new SendProfileRequest(likedUser.ChatId, likedUser.ImagePath, likedUser.UserName,
            likedUser.Age, likedUser.City, likedUser.Description, lng);
    }

    public async Task HandleUserProfileRequestAsync(Update update, string lng)
    {
        var user = await mediatR.Send(new GetUserByIdQuery(update.Message.Chat.Id));

        var userProfileRequest = new Domain.DTO.Request.SendProfileRequest(
            update.Message.Chat.Id,
            user._value.ImagePath,
            user._value.UserName,
            user._value.Age,
            mapLocationService.GetCityByCoordinatesAsync(user._value.Coordinates).Result._value,
            user._value.Description,
            lng);

        logger.LogInformation("Sending user profile to view for chat ID: {ChatId} with user: {UserName}",
                               update.Message.Chat.Id, user._value.UserName);

        await profileRequestBuilder.SetShowProfilePreview(user._value.ChatId, lng).SendAsync();
        await SendProfileAsync(userProfileRequest);
    }

    public async Task SendProfileAsync(SendProfileRequest request)
    {
        logger.LogInformation("Sending profile for ChatId: {ChatId}, UserName: {UserName}", request.ChatId, request.UserName);

        var text = $"{request.UserName}, {request.Age}, {request.MessageCity} ";
        if (request.Description is not null && request.Description != "")
            text += $", {request.Description}";

        await photoHelper.SendPhotoAsync(request.ChatId, request.FilePath, text);
        await mediatR.Send(new AddLastMessageTgBotInCacheCommand(request.ChatId, text));
        logger.LogInformation("Profile sent successfully for ChatId: {ChatId}", request.ChatId);
    }

    public async Task SendProfileByEstimationAsync(SendProfileRequest request)
    {
        var text = $"{request.UserName}, {request.Age}, {request.MessageCity} ";
        if (request.Description is not null && request.Description != "")
            text += $", {request.Description}";
        await mediatR.Send(new AddLastMessageTgBotInCacheCommand(request.ChatId, text));
        await photoHelper.SendPhotoAsync(request.ChatId, request.FilePath, text, ["❤️", "👎", "💤"]);
    }
}
