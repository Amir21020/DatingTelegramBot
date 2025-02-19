namespace DatingTelegramBot.Application.Services.Abstractions.Builder;

public interface IPhotoRequestBuilder : IMessageBuilder
{
    IMessageBuilder SetPhotoMessageRequest(long chatId, string lng);
    IMessageBuilder SetInvalidPhotoMessageRequest(long chatId, string lng);
    IMessageBuilder SetUpdatePhotoMessageRequest(long chatId, string lng);
    IMessageBuilder SetUpdateInvalidPhotoMessageRequest(long chatId, string lng);
}
