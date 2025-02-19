namespace DatingTelegramBot.Application.Services.Abstractions.Builder;

public interface IAgeRequestBuilder : IMessageBuilder
{
    IMessageBuilder SetAgeConfirmationRequest(long chatId, string lng);
    IMessageBuilder SetAgeRequest(long chatId, string lng);
}
