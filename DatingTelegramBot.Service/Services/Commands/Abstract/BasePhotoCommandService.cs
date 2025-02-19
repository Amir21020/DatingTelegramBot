using DatingTelegramBot.Domain.Errors.Service.User;
using DatingTelegramBot.Domain.Errors;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types.Enums;
using DatingTelegramBot.Application.Helpers;
using DatingTelegramBot.Application.Services.Abstractions.Builder;
using Telegram.Bot.Types;

namespace DatingTelegramBot.Service.Services.Commands.Abstract;

public abstract class BasePhotoCommandService(IPhotoRequestBuilder photoRequestBuilder, IPhotoHelper photoHelper, ILogger<BasePhotoCommandService> logger)
{
    protected readonly IPhotoRequestBuilder _photoRequestBuilder = photoRequestBuilder;
    protected readonly IPhotoHelper _photoHelper = photoHelper;
    protected readonly ILogger<BasePhotoCommandService> _logger = logger;
    protected async Task<Result<Photo, Error>> HandlePhotoUpdateAsync(Update update, string lng)
    {
        var chatId = update.Message.Chat.Id;
        if (update.Message.Type == MessageType.Photo)
        {
            logger.LogInformation("Retrieving photo for ChatId: {ChatId}", chatId);
            await photoHelper.DownloadPhotoAsync(update);
            var path = photoHelper.GetPathPhoto(update);
            logger.LogInformation("Photo retrieved for ChatId: {ChatId}, Path: {Path}", chatId, path._value);
            return new Photo(path._value);
        }
        return UserRegistrationErrors.InvalidMessageTypeError;
    }
}
