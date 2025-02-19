namespace DatingTelegramBot.Application.Services.Abstractions.Builder;

public interface IProfileRequestBuilder : IMessageBuilder
{
    IMessageBuilder SetViewQuestRequest(long chatId, string lng);
    IMessageBuilder SetNameRequest(long chatId, string userName, string lng);
    IMessageBuilder SetUserConfirmation(long chatId, string lng);
    IMessageBuilder SetGenderRequest(long chatId, string lng);
    IMessageBuilder SetInvalidOptionMessage(long chatId, string lng);
    IMessageBuilder SetInteresGenderRequest(long chatId, string lng);
    IMessageBuilder SetShowProfileOptions(long chatId, string lng);
    IMessageBuilder SetShowProfilePreview(long chatId, string lng);
    IMessageBuilder SetInvalidNameMessage(long chatId, string userName, string lng);
}
