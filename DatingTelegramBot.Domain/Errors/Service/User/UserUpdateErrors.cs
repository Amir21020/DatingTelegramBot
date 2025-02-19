namespace DatingTelegramBot.Domain.Errors.Service.User;

public static class UserUpdateErrors
{
    public static Error PhotoUpdateCancelledError
        = new("UserUpdate.PhotoUpdateCancelled", "The photo update has been cancelled and is not required.");
    public static Error DescriptionUpdateCancelledError
        = new("UserUpdate.DescriptionUpdateCancelled", "The description update has been cancelled and is not required.");
}
