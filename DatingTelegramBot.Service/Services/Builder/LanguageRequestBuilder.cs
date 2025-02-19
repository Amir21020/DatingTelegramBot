using DatingTelegramBot.Application.Helpers;
using DatingTelegramBot.Application.Services.Abstractions.Builder;
using DatingTelegramBot.Domain.Abstractions;
using DatingTelegramBot.Service.Services.Builder.Abstract;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DatingTelegramBot.Service.Services.Builder;

public sealed class LanguageRequestBuilder
    (IBot bot, IButtonCommandHelper buttonCommandHelper,
    IMediator mediatR,
    ILogger<BaseRequestBuilder> logger): InteractiveMessageBuilder
    (bot, buttonCommandHelper, mediatR, logger), ILanguageRequestBuilder
{
    public IMessageBuilder SetInvalidLanguageRequest(long chatId, string lng)
        => SetMainProfileRequest(chatId, lng, "invalid_option_message", "русский", "english");
    public IMessageBuilder SetLanguageRequest(long chatId, string lng)
        => SetMainProfileRequest(chatId, lng ?? "ru", "language", "русский", "english");
}
