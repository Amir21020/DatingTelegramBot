using DatingTelegramBot.Application.Services.Abstractions.Builder;
using DatingTelegramBot.Domain.Abstractions;
using DatingTelegramBot.Service.Services.Builder.Abstract;
using DatingTelegramBot.Application.Helpers;
using DatingTelegramBot.Service.Helpers;
using Microsoft.Extensions.Logging;
using MediatR;

namespace DatingTelegramBot.Service.Services.Builder;

public sealed class ProfileRequestBuilder(IBot bot,
    IButtonCommandHelper buttonHelpers, ILogger<BaseRequestBuilder> logger, IMediator mediatR)
    : InteractiveMessageBuilder(bot, buttonHelpers, mediatR, logger), IProfileRequestBuilder
{
    public IMessageBuilder SetNameRequest(long chatId, string userName, string lng)
        => SetMainProfileRequest(chatId, lng, "name_request", userName);

    public IMessageBuilder SetInvalidNameMessage(long chatId, string userName, string lng)
        => SetMainProfileRequest(chatId, lng, "enter_valid_name_message", userName);

    public IMessageBuilder SetGenderRequest(long chatId, string lng)
        => SetMainProfileRequest(chatId, lng, "gender_request",
                TranslatorCommandHelper.GetTranslationAsync(lng, "female_option").Result,
                TranslatorCommandHelper.GetTranslationAsync(lng, "male_option").Result
            );

    public IMessageBuilder SetInteresGenderRequest(long chatId, string lng) 
        => SetMainProfileRequest(chatId, lng, "interest_gender_request",
                TranslatorCommandHelper.GetTranslationAsync(lng, "girls_option").Result,
                TranslatorCommandHelper.GetTranslationAsync(lng, "guys_option").Result,
                TranslatorCommandHelper.GetTranslationAsync(lng, "no_preference").Result
            );


    public IMessageBuilder SetUserConfirmation(long chatId, string lng)
        => SetMainProfileRequest(chatId, lng, "confirmation",
            TranslatorCommandHelper.GetTranslationAsync(lng, "yes").Result,
            TranslatorCommandHelper.GetTranslationAsync(lng, "update_photo").Result,
            TranslatorCommandHelper.GetTranslationAsync(lng, "no_preference").Result,
            TranslatorCommandHelper.GetTranslationAsync(lng, "fill_questionnaire_again").Result
    );

    public IMessageBuilder SetShowProfileOptions(long chatId, string lng)
    {
        string[] options = new[]
        {
            TranslatorCommandHelper.GetTranslationAsync(lng, "edit_profile_text").Result,
            TranslatorCommandHelper.GetTranslationAsync(lng, "fill_questionnaire_again").Result,
            TranslatorCommandHelper.GetTranslationAsync(lng, "update_photo").Result,
            TranslatorCommandHelper.GetTranslationAsync(lng, "change_profile_text").Result
        };

        string text = string.Join("\n", options.Select((option, index) => $"{index + 1}. {option}"));

        return SetMainProfileRequest(chatId, lng, text, "1 🚀", "2", "3", "4");
    }

    public IMessageBuilder SetInvalidOptionMessage(long chatId, string lng)
    => SetMainProfileRequest(chatId, lng, "invalid_option_message", "1 🚀", "2", "3", "4");

    public IMessageBuilder SetViewQuestRequest(long chatId, string lng) 
        => SetMessageRequest(chatId, lng, "show_questionnaire");

    public IMessageBuilder SetShowProfilePreview(long chatId, string lng)
        => SetMessageRequest(chatId, lng, "profile_preview_message");
}
