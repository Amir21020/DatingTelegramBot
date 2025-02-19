namespace DatingTelegramBot.Domain.Errors.Repository;

public static class CacheNotFoundErrors
{
    public static readonly Error LanguageIsNotFoundError =
        new("CacheNotFound.LanguageIsNotFound",
            "The selected language is not specified in the cache");
    public static readonly Error CurrentIndexIsNotFoundError =
        new("CacheNotFound.CurrentIndexIsNotFound",
            "Current index not found in cache.");
    public static readonly Error LastCommandIsNotFoundError =
        new("CacheNotFound.LastCommandIsNotFound", "Last command not found in the cache for the specified chat.");
}
