using DatingTelegramBot.Application.Helpers;
using DatingTelegramBot.Application.Services.Abstractions.Builder;
using DatingTelegramBot.Domain.Abstractions;
using DatingTelegramBot.Service.Services.Builder.Abstract;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DatingTelegramBot.Service.Services.Builder;

public sealed class AgeRequestBuilder(IBot bot, ILogger<BaseRequestBuilder> logger, IButtonCommandHelper buttonHelpers,
    IMediator mediatR)
    : BaseRequestBuilder(bot, logger, buttonHelpers, mediatR), IAgeRequestBuilder
{
    public IMessageBuilder SetAgeConfirmationRequest(long chatId, string lng)
        => SetMessageRequest(chatId, lng, "correct_age");


    public IMessageBuilder SetAgeRequest(long chatId, string lng)
        => SetMessageRequest(chatId, lng, "age_request_message");
}
