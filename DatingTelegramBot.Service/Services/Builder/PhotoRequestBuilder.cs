using DatingTelegramBot.Application.Helpers;
using DatingTelegramBot.Application.Services.Abstractions.Builder;
using DatingTelegramBot.Domain.Abstractions;
using DatingTelegramBot.Service.Helpers;
using DatingTelegramBot.Service.Services.Builder.Abstract;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DatingTelegramBot.Service.Services.Builder;

public sealed class PhotoRequestBuilder
    (IBot bot, IButtonCommandHelper buttonHelpers, IMediator mediatR, ILogger<BaseRequestBuilder> logger) : InteractiveMessageBuilder
    (bot,buttonHelpers, mediatR, logger), IPhotoRequestBuilder
{
    public IMessageBuilder SetInvalidPhotoMessageRequest(long chatId, string lng)
        => SetMessageRequest(chatId, lng, "enter_valid_location");

    public IMessageBuilder SetPhotoMessageRequest(long chatId, string lng)
        => SetMessageRequest(chatId, lng, "request_photo");

    public IMessageBuilder SetUpdateInvalidPhotoMessageRequest(long chatId, string lng)
        => SetMainProfileRequest(chatId, lng, "enter_valid_location", TranslatorCommandHelper.GetTranslationAsync(lng, "key_back").Result);

    public IMessageBuilder SetUpdatePhotoMessageRequest(long chatId, string lng)
        => SetMainProfileRequest(chatId, lng, "request_photo", TranslatorCommandHelper.GetTranslationAsync(lng,"key_back").Result);
}
