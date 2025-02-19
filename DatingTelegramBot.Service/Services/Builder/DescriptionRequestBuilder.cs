using DatingTelegramBot.Application.Services.Abstractions.Builder;
using DatingTelegramBot.Domain.Abstractions;
using DatingTelegramBot.Service.Services.Builder.Abstract;
using DatingTelegramBot.Application.Helpers;
using DatingTelegramBot.Service.Helpers;
using Microsoft.Extensions.Logging;
using MediatR;

namespace DatingTelegramBot.Service.Services.Builder;

public sealed class DescriptionRequestBuilder(IBot bot,
    IButtonCommandHelper buttonHelpers,
    IMediator mediatR,
    ILogger<BaseRequestBuilder> logger)
    : InteractiveMessageBuilder(bot, buttonHelpers, mediatR, logger), IDescriptionRequestBuilder
{
    public IMessageBuilder SetDescriptionRequest(long chatId, string lng)
        => SetMainProfileRequest(chatId, lng, "description_request", TranslatorCommandHelper.GetTranslationAsync(lng, "skip").Result);

    public IMessageBuilder SetErrorDescriptionsRequest(long chatId, string lng)
        => SetMainProfileRequest(chatId, lng, "enter_valid_location", TranslatorCommandHelper.GetTranslationAsync(lng, "skip").Result);

    public IMessageBuilder SetUpdateDescriptionRequest(long chatId, string lng)
        => SetMainProfileRequest(chatId, lng, "description_request", TranslatorCommandHelper.GetTranslationAsync(lng, "key_back").Result);

    public IMessageBuilder SetUpdateErrorDescriptionRequest(long chatId, string lng)
        => SetMainProfileRequest(chatId, lng, "enter_valid_location", TranslatorCommandHelper.GetTranslationAsync(lng, "key_back").Result);
}
