namespace DatingTelegramBot.Application.Repository.Options;

public sealed class CachePrefixOptions
{
    public string? CurrentIndexPrefix { get; set; }
    public string? LanguagePrefix { get; set; }
    public string? UserPrefix { get; set; }
}
