namespace DatingTelegramBot.Domain.Errors.Service.User;

public static class UserProfileCommandServiceErrors
{
    public readonly static Error NoProfilesFoundError
        = new("UserProfileCommandService.NoProfilesFound",
            "No user profiles were found for the specified criteria.");

}
