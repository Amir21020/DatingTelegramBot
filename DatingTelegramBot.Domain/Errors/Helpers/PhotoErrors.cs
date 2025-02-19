namespace DatingTelegramBot.Domain.Errors.Helpers;

public static class PhotoErrors
{
    public readonly static Error FilePathNotFoundError =
        new("Photo.FilePathNotFound", "File path for the photo could not be found.");
}


