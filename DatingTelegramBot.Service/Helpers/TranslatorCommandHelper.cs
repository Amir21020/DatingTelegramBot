using Newtonsoft.Json;

namespace DatingTelegramBot.Service.Helpers;

public static class TranslatorCommandHelper
{
    private static readonly Lazy<Task<Dictionary<string, Dictionary<string, string>>>> translations
        = new Lazy<Task<Dictionary<string, Dictionary<string, string>>>>(LoadTranslationsAsync);

    private static async Task<Dictionary<string, Dictionary<string, string>>> LoadTranslationsAsync()
    {
        var path = @"C:\Users\MagicBook 16\source\repos\DatingTelegramBot\DatingTelegramBot.Service\Resources\translations.json";
        var json = await File.ReadAllTextAsync(path);
        return JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(json)
               ?? new Dictionary<string, Dictionary<string, string>>();
    }

    public static async Task<string> GetTranslationAsync(string language, string key)
    {
        var languageDictionary = await translations.Value;

        if (languageDictionary.TryGetValue(language, out var translationsForLanguage) &&
            translationsForLanguage.TryGetValue(key, out var translation))
        {
            return translation;
        }

        return key;
    }
}
