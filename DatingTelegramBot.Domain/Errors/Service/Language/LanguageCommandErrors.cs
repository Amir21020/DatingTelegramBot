namespace DatingTelegramBot.Domain.Errors.Service.Language;

public static class LanguageCommandErrors
{
    public static readonly Error
        InvalidLanguageError = new("LanguageCommand.InvalidLanguage", "The requested language is invalid.");
}
