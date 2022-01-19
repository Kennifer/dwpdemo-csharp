using System.Linq;
using System.Threading.Tasks;
using DWP.Demo.Api.Domain.Filters;
using DWP.Demo.Api.Domain.HttpClient;
using Microsoft.AspNetCore.Mvc;

namespace DWP.Demo.Api.Controllers
{
    public class UserController : Controller
    {
        private readonly IGetUsers _getUsers;
        private readonly IGetUsersByCity _getUsersByCity;
        private readonly IUserDistanceFilter _userDistanceFilter;

        private const string City = "London";
        private const double LondonLatitude = 51.509865;
        private const double LondonLongitude = -0.118092;
        private const double MaxDistance = 50.0;

        public UserController(IGetUsers getUsers, IGetUsersByCity getUsersByCity, IUserDistanceFilter userDistanceFilter)
        {
            _getUsers = getUsers;
            _getUsersByCity = getUsersByCity;
            _userDistanceFilter = userDistanceFilter;
        }

        public async Task<IActionResult> Get()
        {
            var getUsersTask = _getUsers.Execute();
            var getUsersByCityTask = _getUsersByCity.Execute(City);

            await Task.WhenAll(getUsersTask, getUsersByCityTask);

            var removedUsers = _userDistanceFilter.RemoveUsersWithDistanceGreaterThan(
                getUsersTask.Result, LondonLatitude, LondonLongitude, MaxDistance);

            var allUsers = getUsersByCityTask.Result.Concat(removedUsers).Distinct();

            return Ok(allUsers);
        }
    }
}