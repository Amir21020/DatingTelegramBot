using DatingTelegramBot.Application.Helpers;
using DatingTelegramBot.Application.Repository.Commands;
using DatingTelegramBot.Application.Services.Abstractions.Builder;
using DatingTelegramBot.Domain.Abstractions;
using DatingTelegramBot.Service.Helpers;
using MediatR;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace DatingTelegramBot.Service.Services.Builder.Abstract;

public abstract class BaseRequestBuilder(IBot bot, ILogger<BaseRequestBuilder> logger,
    IButtonCommandHelper buttonHelpers,
    IMediator mediatR) : IMessageBuilder
{
    protected readonly IButtonCommandHelper _buttonHelpers = buttonHelpers;
    protected readonly TelegramBotClient _bot = bot.GetTelegramBot().Result;
    protected readonly ChatMessage _chatMessage = new();

    public async Task SendAsync()
    {
        if (_chatMessage.ChatId != 0 && !string.IsNullOrEmpty(_chatMessage.Message))
        {
            await _bot.SendTextMessageAsync(_chatMessage.ChatId, _chatMessage.Message, replyMarkup: _chatMessage.ReplyMarkup);
            await mediatR.Send(new AddLastMessageTgBotInCacheCommand(_chatMessage.ChatId, _chatMessage.Message));
            logger.LogInformation(
            "Message sent to ChatId: {ChatId}. Message: {Message}. ReplyMarkup: {ReplyMarkup}",
                _chatMessage.ChatId,
            _chatMessage.Message,
            _chatMessage.ReplyMarkup);
        }
        else
        {
            logger.LogWarning("Attempt to send message failed. ChatId: {ChatId}, Message is null or empty.", _chatMessage.ChatId);
        }
    }

    protected IMessageBuilder SetMessageRequest(long chatId, string lng, string messageKey)
    {
        _chatMessage.Language = lng;
        _chatMessage.ChatId = chatId;
        _chatMessage.Message = TranslatorCommandHelper.GetTranslationAsync(lng, messageKey).Result;
        _chatMessage.ReplyMarkup = buttonHelpers.CreateRemoveButton();


        logger.LogInformation("Message request set. ChatId: {ChatId}, Language: {Language}, MessageKey: {MessageKey}", chatId, lng, messageKey);
        
        return this;
    }

    protected sealed class ChatMessage
    {
        public string? Message { get; set; }
        public IReplyMarkup? ReplyMarkup { get; set; }
        public string? Language { get; set; }
        public long ChatId { get; set; }
    }
}
