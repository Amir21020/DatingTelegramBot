namespace DatingTelegramBot.Domain.Errors.Service.Map;

public static class MapErrors
{
    public readonly static Error LocationFetchFailedError
        = new ("Map.LocationFetchFailed", "Could not retrieve the location from the API.");
    public readonly static Error AddressFetchFailedError
         = new("Map.AddressFetchFailed", "Could not retrieve the address from the API.");
    public readonly static Error CityNotFoundError = new 
        ("Map.CityNotFound", "City not found.");
}
