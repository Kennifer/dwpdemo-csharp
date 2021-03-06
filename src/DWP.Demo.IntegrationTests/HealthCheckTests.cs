using System.Threading.Tasks;
using NUnit.Framework;

namespace DWP.Demo.IntegrationTests
{
    [TestFixture]
    public class HealthCheckTests
    {
        [Test]
        public async Task HappyPath_ReturnsOk()
        {
            var apiFixture = new ApiFixture(default);
            _ = apiFixture.Start();

            var response = await apiFixture.ApiClient.GetAsync("health");

            Assert.IsTrue(response.IsSuccessStatusCode);
        }
    }
}