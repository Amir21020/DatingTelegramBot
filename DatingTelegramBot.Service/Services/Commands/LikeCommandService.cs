using DatingTelegramBot.Application.Helpers;
using DatingTelegramBot.Application.Services.Abstractions.Commands;
using DatingTelegramBot.Domain.Abstractions;
using DatingTelegramBot.Domain.DTO.Request;
using DatingTelegramBot.Domain.DTO.Responses;
using DatingTelegramBot.Service.Helpers;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace DatingTelegramBot.Service.Services.Commands;

public sealed class LikeCommandService(IBot telegramBotClient,
        IMapLocationService mapLocationService,
        IProfileViewCommandService profileViewCommandService,
        IButtonCommandHelper buttonCommandHelper,
        ILogger<LikeCommandService> logger) : ILikeCommandService
{
    private readonly TelegramBotClient _telegramBotClient = telegramBotClient.GetTelegramBot().Result;
    public async Task HandleLikeAsync(UserProfileResponse likedUser, UserResponse user, string lng)
    {
        logger.LogInformation("Handling like for UserId: {UserId} by ChatId: {ChatId}", user.ChatId, likedUser.ChatId);

        var city = await mapLocationService.GetCityByCoordinatesAsync(user.Coordinates);
        logger.LogInformation("Retrieved city '{City}' for UserId: {UserId}", city._value, user.ChatId);

        await SendProfileAsync(likedUser.ChatId, user, city._value, lng);
        await SendInlineKeyboardAsync(likedUser.ChatId, user.TgUserName, lng);
    }

    private async Task SendProfileAsync(long chatId, UserResponse user, string city, string lng)
    {
        await profileViewCommandService.SendProfileAsync(new SendProfileRequest(
            chatId,
            user.ImagePath,
            user.UserName,
            user.Age,
            city,
            user.Description,
            lng));

        logger.LogInformation("Sent profile for UserId: {UserId} to ChatId: {ChatId}", user.ChatId, chatId);
    }

    private async Task SendInlineKeyboardAsync(long chatId, string tgUserName, string lng)
    {
        var inlineButton = buttonCommandHelper.CreateInlineKeyboard(
            (await TranslatorCommandHelper.GetTranslationAsync(lng, "write"), $"https://t.me/{tgUserName}"));

        await _telegramBotClient.SendTextMessageAsync(chatId, string.Empty, replyMarkup: inlineButton);
        logger.LogInformation("Sent message with inline button to ChatId: {ChatId} for UserId: {UserId}", chatId, tgUserName);
    }
}
