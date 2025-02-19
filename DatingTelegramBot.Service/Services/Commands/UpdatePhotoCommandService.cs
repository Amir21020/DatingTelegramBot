using DatingTelegramBot.Application.Helpers;
using DatingTelegramBot.Application.Services.Abstractions.Builder;
using DatingTelegramBot.Application.Services.Abstractions.Commands;
using DatingTelegramBot.Domain.Errors;
using DatingTelegramBot.Domain.Errors.Service.User;
using DatingTelegramBot.Service.Helpers;
using DatingTelegramBot.Service.Services.Commands.Abstract;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;

namespace DatingTelegramBot.Service.Services.Commands;

public sealed class UpdatePhotoCommandService (IPhotoRequestBuilder photoRequestBuilder, 
    IPhotoHelper photoHelper,
    ILogger<BasePhotoCommandService> logger) : BasePhotoCommandService(photoRequestBuilder,
        photoHelper,logger), IUpdateCommandService<Photo>
{
    public async Task<Result<Photo, Error>> RetrieveMessageAsync(Update update, string lng)
    {
        var chatId = update.Message.Chat.Id;

        if (IsBackCommand(update.Message.Text, lng))
        {
            return UserUpdateErrors.PhotoUpdateCancelledError;
        }

        return await HandlePhotoUpdateAsync(update, lng);
    }

    private bool IsBackCommand(string messageText, string lng)
    {
        var backCommand = TranslatorCommandHelper.GetTranslationAsync(lng, "key_back").Result;
        return messageText == backCommand;
    }

    public async Task SendInvalidMessageAsync(Update update, string lng)
        => await _photoRequestBuilder.SetUpdateInvalidPhotoMessageRequest(update.Message.Chat.Id, lng).SendAsync();

    public async Task SendMessageAsync(Update update, string lng)
        => await _photoRequestBuilder.SetUpdatePhotoMessageRequest(update.Message.Chat.Id, lng).SendAsync();
}
