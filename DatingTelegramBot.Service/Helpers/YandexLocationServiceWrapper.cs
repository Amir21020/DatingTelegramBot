using DatingTelegramBot.Application.Helpers;
using DatingTelegramBot.Domain.Entity;
using DatingTelegramBot.Domain.Errors;
using DatingTelegramBot.Domain.Errors.Service.User;
using DatingTelegramBot.Service.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Globalization;
using Yandex.Geocoder;
using Yandex.Geocoder.Enums;

namespace DatingTelegramBot.Service.Helpers;

public sealed class YandexLocationServiceWrapper(IOptions<MapOptions> mapOptions,
    ILogger<YandexLocationServiceWrapper> logger) : IMapLocationService
{

    public async Task<Result<string, Error>> GetCityByCoordinatesAsync(Coordinates coordinates)
    {
        var request = new ReverseGeocoderRequest
        {
            Latitude = coordinates.Latitude,
            Longitude = coordinates.Longitude,
        };
        var client = new GeocoderClient(mapOptions.Value.YandexApiKey);
        var response = await client.ReverseGeocode(request);
        var firstGeoObject = response.GeoObjectCollection.FeatureMember.FirstOrDefault();
        var addressComponents = firstGeoObject.GeoObject.MetaDataProperty.GeocoderMetaData.Address.Components;
        var city = addressComponents.FirstOrDefault(c => c.Kind.Equals(AddressComponentKind.Locality));
        return city.Name;

    }

    public async Task<Result<Coordinates, Error>> GetLatLongFromAddressAsync(string address)
    {
        var request = new GeocoderRequest { Request = address };
        var client = new GeocoderClient(mapOptions.Value.YandexApiKey);
        var response = await client.Geocode(request);
        var firstGeoObject = response.GeoObjectCollection.FeatureMember.FirstOrDefault();
        var coordinate = firstGeoObject.GeoObject.Point.Pos;

        var pos = coordinate.Split(' ').Select(x => x.Trim()).ToArray();

        if (pos.Length < 2)
        {
            return UserRegistrationErrors.CoordinatesFormatError;
        }

        logger.LogDebug($"Longitude String: {pos[0]}, Latitude String: {pos[1]}");

        if (double.TryParse(pos[0], NumberStyles.Float, CultureInfo.InvariantCulture, out double longitude) &&
            double.TryParse(pos[1], NumberStyles.Float, CultureInfo.InvariantCulture, out double latitude))
        {
            return new Coordinates(latitude, longitude);
        }
        else
        {
            logger.LogDebug($"Failed to parse longitude '{pos[0]}' or latitude '{pos[1]}'");
            return UserRegistrationErrors.CoordinatesFormatError;
        }

    }
}
