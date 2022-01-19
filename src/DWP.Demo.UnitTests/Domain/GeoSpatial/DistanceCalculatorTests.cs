using System;
using DWP.Demo.Api.Domain.GeoSpatial.Impelentation;
using NUnit.Framework;

namespace DWP.Demo.UnitTests.Domain.GeoSpatial
{
    [TestFixture]
    public class DistanceCalculatorTests
    {
        // Test Data from: https://www.meridianoutpost.com/resources/etools/calculators/calculator-latitude-longitude-distance.php
        
        [TestCase(1.0, 1.0, 2.0, 2.0, 97.69)]
        [TestCase(53.483959, -2.244644, 53.483959, -2.244644, 0.0)]
        [TestCase(53.483959, -2.244644, 70, -2.244644, 1141.09)] // North
        [TestCase(53.483959, -2.244644, 70, 4, 1157.79)] // North West
        [TestCase(53.483959, -2.244644, 53.483959, 10, 502.78)] // East
        [TestCase(53.483959, -2.244644, -10.123456, 10.123456, 4454.49)] // South East
        [TestCase(53.483959, -2.244644, -53.483959, -2.244644, 7390.41)] // South
        [TestCase(53.483959, -2.244644, -53.483959, -10, 7403.82)]  // South West
        [TestCase(53.483959, -2.244644, 53.483959, -10, 318.68)] // West
        [TestCase(53.483959, -2.244644, 54, -10, 318.73)] // North West
        public void DistanceBetween_IsCorrect(double lat1, double lon1, double lat2, double lon2, double expectedDistance)
        {
            var sut = new DistanceCalculator();

            var result = sut.DistanceBetween(lat1, lon1, lat2, lon2);

            Assert.That(Math.Round(result, 2), Is.EqualTo(Math.Round(expectedDistance, 2)));
        }
    }
}