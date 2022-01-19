using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DWP.Demo.Api.Domain.Filters;
using DWP.Demo.Api.Domain.HttpClient;
using DWP.Demo.Api.Types;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace DWP.Demo.UnitTests.Controllers
{
    [TestFixture]
    public class UsersController
    {
        [Test]
        public async Task Get_HappyPath()
        {
            const string expectedCity = "London";
            const double expectedDistance = 50.0;
            const double expectedLatitude = 51.509865;
            const double expectedLongitude = -0.118092;

            const int user1Id = 1;
            const int user2Id = 2;
            
            var getUsers = new[] { new User() { Id = user1Id } };
            var cityUsers = new[] { new User() { Id = user2Id } };
            var filteredResults = cityUsers;
            
            var getUsersQuery = new Mock<IGetUsers>();
            getUsersQuery.Setup(x => x.Execute())
                .ReturnsAsync(getUsers);
            
            var getUsersByCity = new Mock<IGetUsersByCity>();
            getUsersByCity.Setup(x => x.Execute(expectedCity))
                .ReturnsAsync(cityUsers);
            
            var userFilter = new Mock<IUserDistanceFilter>();
            userFilter.Setup(x =>
                    x.RemoveUsersWithDistanceGreaterThan(cityUsers, expectedLatitude, expectedLongitude,
                        expectedDistance))
                .Returns(filteredResults);
            
            var sut = new UserController(getUsersQuery.Object, getUsersByCity.Object, userFilter.Object);

            var result = await sut.Get();

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okObjectResult = result as OkObjectResult;
            
            Assert.That(okObjectResult.Value, Is.InstanceOf<IEnumerable<User>>());
            var users = okObjectResult.Value as IEnumerable<User>;

            Assert.That(users.Count(), Is.EqualTo(2));
            Assert.That(users, Has.Exactly(1).Property(nameof(User.Id)).EqualTo(user1Id));
            Assert.That(users, Has.Exactly(1).Property(nameof(User.Id)).EqualTo(user2Id));
        }
    }

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
                getUsersByCityTask.Result, LondonLatitude, LondonLongitude, MaxDistance);

            var allUsers = getUsersTask.Result.Concat(removedUsers);

            return Ok(allUsers);
        }
    }
}