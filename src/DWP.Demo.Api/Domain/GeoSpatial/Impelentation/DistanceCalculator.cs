using System;

namespace DWP.Demo.Api.Domain.GeoSpatial.Impelentation
{
    public class DistanceCalculator : IDistanceCalculator
    {
        // Credit goes to:
        // https://stackoverflow.com/a/24712129/279579
        
        public double DistanceBetween(double lat1, double lon1, double lat2, double lon2)
        {
            double rlat1 = Math.PI*lat1/180;
            double rlat2 = Math.PI*lat2/180;
            double theta = lon1 - lon2;
            double rtheta = Math.PI*theta/180;
            double dist =
                Math.Sin(rlat1)*Math.Sin(rlat2) + Math.Cos(rlat1)*
                Math.Cos(rlat2)*Math.Cos(rtheta);
            dist = Math.Acos(dist);
            dist = dist*180/Math.PI;
            dist = dist*60*1.1515;
                    
            return dist;
        }
    }
}