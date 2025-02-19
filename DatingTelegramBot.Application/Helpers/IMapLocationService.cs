using DatingTelegramBot.Domain.Entity;
using DatingTelegramBot.Domain.Errors;
namespace DatingTelegramBot.Application.Helpers;

public interface IMapLocationService
{
    Task<Result<Coordinates, Error>> GetLatLongFromAddressAsync(string address);
    Task<Result<string, Error>> GetCityByCoordinatesAsync(Coordinates coordinates);
}
