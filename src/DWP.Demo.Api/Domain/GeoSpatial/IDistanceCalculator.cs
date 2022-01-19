namespace DWP.Demo.Api.Domain.GeoSpatial
{
    public interface IDistanceCalculator
    {
        double DistanceBetween(double lat1, double lon1, double lat2, double lon2);
    }
}