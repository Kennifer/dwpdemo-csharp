using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DWP.Demo.Api.Controllers;
using DWP.Demo.Api.Domain.Filters;
using DWP.Demo.Api.Domain.HttpClient;
using DWP.Demo.Api.Types;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace DWP.Demo.UnitTests.Controllers
{
    [TestFixture]
    public class UsersControllerTests
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
            
            var cityUsers = new[] { new User() { Id = user2Id } };
            var getUsers = new[] { new User() { Id = user1Id } };
            var filteredResults = getUsers;
            
            var getUsersQuery = new Mock<IGetUsers>();
            getUsersQuery.Setup(x => x.Execute())
                .ReturnsAsync(getUsers);
            
            var getUsersByCity = new Mock<IGetUsersByCity>();
            getUsersByCity.Setup(x => x.Execute(expectedCity))
                .ReturnsAsync(cityUsers);
            
            var userFilter = new Mock<IUserDistanceFilter>();
            userFilter.Setup(x =>
                    x.RemoveUsersWithDistanceGreaterThan(getUsers, expectedLatitude, expectedLongitude,
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

        [Test]
        public async Task Get_UsersRemovedDueToDistance()
        {
            const string expectedCity = "London";
            const double expectedDistance = 50.0;
            const double expectedLatitude = 51.509865;
            const double expectedLongitude = -0.118092;

            const int user1Id = 1;
            const int user2Id = 2;
            const int user3Id = 3;
            
            var cityUsers = new[] { new User() { Id = user1Id } };
            var getUsers = new[] { new User() { Id = user2Id }, new User() { Id = user3Id } };
            var filteredResults = new[] { new User() { Id = user3Id } };
            
            var getUsersQuery = new Mock<IGetUsers>();
            getUsersQuery.Setup(x => x.Execute())
                .ReturnsAsync(getUsers);
            
            var getUsersByCity = new Mock<IGetUsersByCity>();
            getUsersByCity.Setup(x => x.Execute(expectedCity))
                .ReturnsAsync(cityUsers);
            
            var userFilter = new Mock<IUserDistanceFilter>();
            userFilter.Setup(x =>
                    x.RemoveUsersWithDistanceGreaterThan(getUsers, expectedLatitude, expectedLongitude,
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
            Assert.That(users, Has.Exactly(1).Property(nameof(User.Id)).EqualTo(user3Id));
        }
        
        [Test]
        public async Task Get_DuplicateUsers_HasRemovesDuplicates()
        {
            const string expectedCity = "London";
            const double expectedDistance = 50.0;
            const double expectedLatitude = 51.509865;
            const double expectedLongitude = -0.118092;

            const int user1Id = 1;
            
            var getUsers = new[] { new User() { Id = user1Id } };
            var cityUsers = new[] { new User() { Id = user1Id } };
            var filteredResults = new[] { new User() { Id = user1Id } };
            
            var getUsersQuery = new Mock<IGetUsers>();
            getUsersQuery.Setup(x => x.Execute())
                .ReturnsAsync(getUsers);
            
            var getUsersByCity = new Mock<IGetUsersByCity>();
            getUsersByCity.Setup(x => x.Execute(expectedCity))
                .ReturnsAsync(cityUsers);
            
            var userFilter = new Mock<IUserDistanceFilter>();
            userFilter.Setup(x =>
                    x.RemoveUsersWithDistanceGreaterThan(getUsers, expectedLatitude, expectedLongitude,
                        expectedDistance))
                .Returns(filteredResults);
            
            var sut = new UserController(getUsersQuery.Object, getUsersByCity.Object, userFilter.Object);

            var result = await sut.Get();

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okObjectResult = result as OkObjectResult;
            
            Assert.That(okObjectResult.Value, Is.InstanceOf<IEnumerable<User>>());
            var users = okObjectResult.Value as IEnumerable<User>;

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users, Has.Exactly(1).Property(nameof(User.Id)).EqualTo(user1Id));
        }

        [Test]
        public async Task Get_CombinesCorrectUsers()
        {
            const string expectedCity = "London";
            const double expectedDistance = 50.0;
            const double expectedLatitude = 51.509865;
            const double expectedLongitude = -0.118092;

            const int user1Id = 1;
            const int user2Id = 2;
            const int user3Id = 3;
            
            var getUsers = new[] { new User() { Id = user1Id }, new User() { Id = user2Id } };
            var cityUsers = new[] { new User() { Id = user3Id } };
            var filteredResults = new[] { new User() { Id = user1Id } };
            
            var getUsersQuery = new Mock<IGetUsers>();
            getUsersQuery.Setup(x => x.Execute())
                .ReturnsAsync(getUsers);
            
            var getUsersByCity = new Mock<IGetUsersByCity>();
            getUsersByCity.Setup(x => x.Execute(expectedCity))
                .ReturnsAsync(cityUsers);
            
            var userFilter = new Mock<IUserDistanceFilter>();
            userFilter.Setup(x =>
                    x.RemoveUsersWithDistanceGreaterThan(getUsers, expectedLatitude, expectedLongitude,
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
            Assert.That(users, Has.Exactly(1).Property(nameof(User.Id)).EqualTo(user3Id));
        }
    }
}