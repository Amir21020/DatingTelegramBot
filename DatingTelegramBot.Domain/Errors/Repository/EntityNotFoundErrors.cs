namespace DatingTelegramBot.Domain.Errors.Repository;

public static class EntityNotFoundErrors
{
    public static readonly Error UserNotFoundError =
        new("EntityNotFound.UserNotFound",
            "The specified user was not found");
}
