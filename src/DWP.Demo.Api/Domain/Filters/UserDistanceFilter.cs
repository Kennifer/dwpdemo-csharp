using System.Collections.Generic;
using System.Linq;
using DWP.Demo.Api.Domain.GeoSpatial;
using DWP.Demo.Api.Types;

namespace DWP.Demo.Api.Domain.Filters
{
    public class UserDistanceFilter : IUserDistanceFilter
    {
        private readonly IDistanceCalculator _distanceCalculator;

        public UserDistanceFilter(IDistanceCalculator distanceCalculator)
        {
            _distanceCalculator = distanceCalculator;
        }

        public IEnumerable<User> RemoveUsersWithDistanceGreaterThan(
            IEnumerable<User> users,
            double fromLatitude,
            double fromLongitude,
            double maxDistance)
            => users.Where(user =>
                _distanceCalculator.DistanceBetween(fromLatitude, fromLongitude, user.Latitude, user.Longitude) <=
                maxDistance);
    }
}