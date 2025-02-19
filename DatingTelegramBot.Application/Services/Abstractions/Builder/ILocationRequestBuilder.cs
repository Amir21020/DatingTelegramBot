namespace DatingTelegramBot.Application.Services.Abstractions.Builder;

public interface ILocationRequestBuilder : IMessageBuilder
{
    IMessageBuilder SetLocationRequest(long chatId, string lng);
    IMessageBuilder SetInvalidDataError(long chatId, string lng);
}
