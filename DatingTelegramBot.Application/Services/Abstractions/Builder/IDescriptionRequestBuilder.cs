namespace DatingTelegramBot.Application.Services.Abstractions.Builder;

public interface IDescriptionRequestBuilder : IMessageBuilder
{
    IMessageBuilder SetDescriptionRequest(long chatId, string lng);
    IMessageBuilder SetUpdateDescriptionRequest(long chatId, string lng);
    IMessageBuilder SetUpdateErrorDescriptionRequest(long chatId, string lng);
    IMessageBuilder SetErrorDescriptionsRequest(long chatId, string lng);
}