using DatingTelegramBot.Application.Helpers;
using DatingTelegramBot.Application.Services.Abstractions.Builder;
using DatingTelegramBot.Domain.Abstractions;
using DatingTelegramBot.Service.Helpers;
using DatingTelegramBot.Service.Services.Builder.Abstract;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DatingTelegramBot.Service.Services.Builder;

public sealed class LocationRequestBuilder(IBot bot, IButtonCommandHelper buttonHelpers,
    ILogger<BaseRequestBuilder> logger, IMediator mediatR) 
    : InteractiveMessageBuilder(bot, buttonHelpers, mediatR, logger), ILocationRequestBuilder
{
    public IMessageBuilder SetInvalidDataError(long chatId, string lng)
        => SetLocationButtonRequest(chatId, "enter_valid_location", lng);
    public IMessageBuilder SetLocationRequest(long chatId, string lng)
        => SetLocationButtonRequest(chatId, "location_request", lng);



    private IMessageBuilder SetLocationButtonRequest(long chatId,string messageKey, string lng)
    {
        _chatMessage.Language = lng;
        _chatMessage.ChatId = chatId;

        _chatMessage.Message = TranslatorCommandHelper.GetTranslationAsync(lng, messageKey).Result;

        var locationButton = _buttonHelpers.CreateLocationButton(TranslatorCommandHelper.GetTranslationAsync(lng, "send_my_location").Result);
        _chatMessage.ReplyMarkup = locationButton;

        return this;
    }
}
