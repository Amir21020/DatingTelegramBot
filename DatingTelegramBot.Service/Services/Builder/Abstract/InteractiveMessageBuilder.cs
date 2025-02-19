using DatingTelegramBot.Application.Helpers;
using DatingTelegramBot.Application.Services.Abstractions.Builder;
using DatingTelegramBot.Domain.Abstractions;
using DatingTelegramBot.Service.Helpers;
using MediatR;
using Microsoft.Extensions.Logging;



namespace DatingTelegramBot.Service.Services.Builder.Abstract;

public abstract class InteractiveMessageBuilder(IBot bot, 
    IButtonCommandHelper buttonHelpers,
    IMediator mdMediator,
    ILogger<BaseRequestBuilder> logger) : BaseRequestBuilder(bot, logger, buttonHelpers,mdMediator)
{
    protected IMessageBuilder SetMainProfileRequest(long chatId, string lng, string requestMessageKey, params string[] options)
    {
        _chatMessage.Language = lng;
        _chatMessage.ChatId = chatId;
        _chatMessage.Message = TranslatorCommandHelper.GetTranslationAsync(lng, requestMessageKey).Result;

        var keyboard = _buttonHelpers.CreateMainKeyboard(options);
        _chatMessage.ReplyMarkup = keyboard;

        return this;
    }

    protected IMessageBuilder SetInlineProfileRequest(long chatId, string lng, string requestMessageKey, string username)
    {
        _chatMessage.Language = lng;
        _chatMessage.ChatId = chatId;

        _chatMessage.Message = TranslatorCommandHelper.GetTranslationAsync(lng, requestMessageKey).Result;

        var inlineButton = _buttonHelpers.CreateInlineKeyboard(
            (TranslatorCommandHelper.GetTranslationAsync(lng, "write").Result, $"https://t.me/{username}"));

        _chatMessage.ReplyMarkup = inlineButton;

        return this;
    }

}
