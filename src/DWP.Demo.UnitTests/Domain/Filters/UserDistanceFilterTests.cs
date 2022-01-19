using System.Linq;
using DWP.Demo.Api.Domain.Filters;
using DWP.Demo.Api.Domain.GeoSpatial;
using DWP.Demo.Api.Types;
using Moq;
using NUnit.Framework;

namespace DWP.Demo.UnitTests.Domain.Filters
{
    [TestFixture]
    public class UserDistanceFilterTests
    {
        [Test]
        public void FindUsersWithinDistanceOf_ReturnsResults()
        {
            const int validUserId = 1;
            const int invalidUserId = 2;
            const double sourceLatitude = 10.0;
            const double sourceLongitude = 11.0;
            const double validUserLatitude = 20.0;
            const double validUserLongitude = 21.0;
            const double invalidUserLatitude = 30.0;
            const double invalidUserLongitude = 31.0;

            const double validDistance = 39.9;
            const double distance = 40.0;
            const double invalidDistance = 40.1;
            
            var users = new[]
            {
                new User()
                {
                    Id = validUserId,
                    Latitude = validUserLatitude,
                    Longitude = validUserLongitude
                },
                new User()
                {
                    Id = invalidUserId,
                    Latitude = invalidUserLatitude,
                    Longitude = invalidUserLongitude
                }
            };

            var distanceCalculator = new Mock<IDistanceCalculator>();

            distanceCalculator.Setup(x =>
                    x.DistanceBetween(sourceLatitude, sourceLongitude, validUserLatitude, validUserLongitude))
                .Returns(validDistance);
            distanceCalculator.Setup(x =>
                    x.DistanceBetween(sourceLatitude, sourceLongitude, invalidUserLatitude, invalidUserLongitude))
                .Returns(invalidDistance);

            var sut = new UserDistanceFilter(distanceCalculator.Object);

            var result = sut.RemoveUsersWithDistanceGreaterThan(users, sourceLatitude, sourceLongitude, distance);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.Single().Id, Is.EqualTo(validUserId));
        }
    }
}