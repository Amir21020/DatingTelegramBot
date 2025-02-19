namespace DatingTelegramBot.Application.Services.Abstractions.Builder;

public interface ILanguageRequestBuilder : IMessageBuilder
{
    IMessageBuilder SetLanguageRequest(long chatId, string lng);
    IMessageBuilder SetInvalidLanguageRequest(long chatId, string lng);
}
