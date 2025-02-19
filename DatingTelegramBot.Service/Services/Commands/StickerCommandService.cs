using DatingTelegramBot.Application.Helpers;
using DatingTelegramBot.Application.Repository.Commands;
using DatingTelegramBot.Application.Repository.Query;
using DatingTelegramBot.Application.Services.Abstractions.Builder;
using DatingTelegramBot.Application.Services.Abstractions.Commands;
using DatingTelegramBot.Domain.DTO.Request;
using DatingTelegramBot.Domain.DTO.Responses;
using MediatR;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;

namespace DatingTelegramBot.Service.Services.Commands;

public sealed class StickerCommandService(
    IProfileViewCommandService profileViewCommandService,
    IMediator mediatR,
    ILogger<StickerCommandService> logger,
    IMediaRequestBuilder mediaRequestBuilder,
    IMapLocationService mapLocationService) : IStickerCommandService
{
    public async Task HandleDislikeAsync(Update update, string lng)
    {
        var chatId = update.Message.Chat.Id;
        var users = await mediatR.Send(new GetUserProfilesByLocationAndInterestByIdQuery(chatId));

        if (users._value.Length == 0)
        {
            await HandleNoProfilesFound(chatId, lng);
            return;
        }
        int currentIndex = new Random().Next(0, users._value.Length);
        await mediatR.Send(new SetCurrentIndexInCacheCommmand(chatId, currentIndex));

        var user = await profileViewCommandService.GetViewUserProfileAsync(update, currentIndex, lng);

        if (user._error is not null)
        {
            await HandleNoProfilesFound(chatId, lng);
            return;
        }
        await profileViewCommandService.SendProfileByEstimationAsync(new SendProfileRequest(chatId, user._value.FilePath, user._value.UserName,
            user._value.Age, user._value.MessageCity, user._value.Description, user._value.Language));
    }

    public async Task HandleLikeAsync(int currentIndex, Update update, string lng)
    {
        var likedUser = await profileViewCommandService.GetViewUserProfileAsync(update, currentIndex, lng);
        var user = await mediatR.Send(new GetUserByIdQuery(update.Message.Chat.Id));

        logger.LogInformation("Handling like for UserId: {UserId} by ChatId: {ChatId}", user._value.ChatId, likedUser._value.ChatId);

        var city = await mapLocationService.GetCityByCoordinatesAsync(user._value.Coordinates);

        await profileViewCommandService.SendProfileAsync(new(likedUser._value.ChatId, user._value.ImagePath,
            user._value.UserName, user._value.Age, city._value, user._value.Description, lng));

        await SendLikedProfileNotification(likedUser._value, user._value, lng);
        await HandleDislikeAsync(update, lng);
    }

    private async Task HandleNoProfilesFound(long chatId, string lng)
    {
        logger.LogWarning("No users found for ChatId: {ChatId}", chatId);
        await mediaRequestBuilder.SetNotProfilesForSending(chatId, lng).SendAsync();
    }

    private async Task SendLikedProfileNotification(SendProfileRequest likedUser, UserResponse user, string lng)
    {
        await mediaRequestBuilder.SetLikedProfileNotification(likedUser.ChatId, lng, user.TgUserName).SendAsync();
        logger.LogInformation("Sent message with inline button to ChatId: {ChatId} for UserId: {UserId}", likedUser.ChatId, user.ChatId);
    }
}
