using DatingTelegramBot.Domain.Entity;

namespace DatingTelegramBot.Application.Helpers;

public static class DistanceCalculatorHelper
{
    private const double EarthRadiusKm = 6371.0;
    public static double CalculateDistance(Coordinates loc1, Coordinates loc2)
    {
        double lat1Rad = DegreesToRadians(loc1.Latitude);
        double lon1Rad = DegreesToRadians(loc1.Longitude);
        double lat2Rad = DegreesToRadians(loc2.Latitude);
        double lon2Rad = DegreesToRadians(loc2.Longitude);

        double dLat = lat2Rad - lat1Rad;
        double dLon = lon2Rad - lon1Rad;

        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                   Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                   Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return EarthRadiusKm * c;
    }

    private static double DegreesToRadians(double degrees)
    {
        return degrees * Math.PI / 180;
    }
}
