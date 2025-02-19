namespace DatingTelegramBot.Application.Services.Abstractions.Builder;

public interface IMediaRequestBuilder : IMessageBuilder
{
    IMessageBuilder SetMediaRequest(long chatId, string lng);
    IMessageBuilder SetNotProfilesForSending(long chatId, string lng);
    IMessageBuilder SetLikedProfileNotification(long chatId, string lng, string username);
}
