using System.Collections.Generic;
using DWP.Demo.Api.Types;

namespace DWP.Demo.Api.Domain.Filters
{
    public interface IUserDistanceFilter
    {
        IEnumerable<User> RemoveUsersWithDistanceGreaterThan(
            IEnumerable<User> users,
            double fromLatitude, 
            double fromLongitude, 
            double maxDistance);
    }
}