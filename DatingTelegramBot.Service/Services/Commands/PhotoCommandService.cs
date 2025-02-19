using DatingTelegramBot.Application.Helpers;
using DatingTelegramBot.Application.Services.Abstractions.Builder;
using DatingTelegramBot.Application.Services.Abstractions.Commands;
using DatingTelegramBot.Domain.Errors;
using DatingTelegramBot.Service.Services.Commands.Abstract;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;

namespace DatingTelegramBot.Service.Services.Commands;

public sealed class PhotoCommandService(IPhotoRequestBuilder photoRequestBuilder,
        IPhotoHelper photoHelper,
        ILogger<BasePhotoCommandService> logger) : BasePhotoCommandService(photoRequestBuilder, photoHelper,
            logger), ICommandService<Photo>
{
    public async Task SendMessageAsync(Update update, string lng)
        => await photoRequestBuilder.SetPhotoMessageRequest(update.Message.Chat.Id, lng).SendAsync();

    public async Task SendInvalidMessageAsync(Update update, string lng)
        => await photoRequestBuilder.SetInvalidPhotoMessageRequest(update.Message.Chat.Id, lng).SendAsync();

    public async Task<Result<Photo, Error>> RetrieveMessageAsync(Update update, string lng)
        => await HandlePhotoUpdateAsync(update,lng);
}

public sealed class Photo(string pathPhoto)
{
    public override string ToString()
    {
        return pathPhoto;
    }
}