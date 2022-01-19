using System.Collections.Generic;
using System.Linq;
using DWP.Demo.Api.Domain.GeoSpatial;
using DWP.Demo.Api.Types;
using Moq;
using NUnit.Framework;

namespace DWP.Demo.UnitTests.Service
{
    [TestFixture]
    public class GeoSpatialServiceTests
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

            var sut = new GeoSpatialService(distanceCalculator.Object);

            var result = sut.RemoveUsersWithDistanceGreaterThanSource(users, sourceLatitude, sourceLongitude, distance);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.Single().Id, Is.EqualTo(validUserId));
        }
    }

    public interface IGeoSpatialService
    {
        IEnumerable<User> RemoveUsersWithDistanceGreaterThanSource(IEnumerable<User> users, double latitude, double longitude, double distance);
    }

    public class GeoSpatialService : IGeoSpatialService
    {
        private readonly IDistanceCalculator _distanceCalculator;

        public GeoSpatialService(IDistanceCalculator distanceCalculator)
        {
            _distanceCalculator = distanceCalculator;
        }

        public IEnumerable<User> RemoveUsersWithDistanceGreaterThanSource(IEnumerable<User> users, double latitude,
            double longitude, double distance)
        {
            var toReturn = new List<User>();
            foreach (var user in users)
            {
                var distanceBetween =
                    _distanceCalculator.DistanceBetween(latitude, longitude, user.Latitude, user.Longitude);

                if (distanceBetween <= distance)
                {
                    toReturn.Add(user);
                }
            }

            return toReturn;
        }
    }
}