using DatingTelegramBot.Application.Helpers;
using DatingTelegramBot.Application.Services.Abstractions.Builder;
using DatingTelegramBot.Domain.Abstractions;
using DatingTelegramBot.Service.Helpers;
using DatingTelegramBot.Service.Services.Builder.Abstract;
using DatingTelegramBot.Service.Services.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DatingTelegramBot.Service.Services.Builder;

public sealed class MediaRequestBuilder(IBot bot,
    IButtonCommandHelper buttonHelpers, ILogger<BaseRequestBuilder> logger, IMediator mediatR)
    : InteractiveMessageBuilder(bot, buttonHelpers, mediatR, logger), IMediaRequestBuilder
{
    public IMessageBuilder SetMediaRequest(long chatId, string lng)
        => SetMainProfileRequest(chatId, lng, "media_request", TranslatorCommandHelper.GetTranslationAsync(lng, "key_back").Result);

    public IMessageBuilder SetNotProfilesForSending(long chatId, string lng)
        => SetMessageRequest(chatId, lng, "no_profiles_found_message");


    public IMessageBuilder SetLikedProfileNotification(long chatId, string lng, string username)
        => SetInlineProfileRequest(chatId, lng, "you_liked_this_person", username);
}
