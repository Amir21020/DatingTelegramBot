namespace DatingTelegramBot.Domain.Errors.Service.User;

public static class UserRegistrationErrors
{
    public static Error AgeFormatError
        = new("UserRegistration.AgeFormat", "incorrect age input format");
    public static Error GenderFormatError 
         = new("UserRegistration.GenderFormat", "incorrect gender input format");
    public static Error GenderSearchInterestError 
        = new("UserRegistration.GenderSearchInterest", "incorrect gender search interest input");
    public static Error UserNameIsNotFoundError
        = new("UserRegistration.UserNameIsNotFound", "User with the specified name was not found.");
    public static Error CoordinatesFormatError
      = new("UserRegistration.CoordinatesFormat", "incorrect coordinates input format");
    public static Error InvalidMessageTypeError
        = new("UserRegistration.InvalidMessageType", "The message type is not supported.");
    public static Error InvalidMessageError
        = new("UserRegistration.InvalidMessage", "The content of the message is invalid.");
}
