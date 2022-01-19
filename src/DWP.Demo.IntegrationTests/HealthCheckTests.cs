using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DWP.Demo.IntegrationTests
{
    [TestClass]
    public class HealthCheckTests
    {
        [TestMethod]
        public async Task HappyPath_ReturnsOk()
        {
            var apiFixture = new ApiFixture(default);
            _ = apiFixture.Start();

            var response = await apiFixture.ApiClient.GetAsync("health");

            Assert.IsTrue(response.IsSuccessStatusCode);
        }
    }

    [TestClass]
    public class UsersController
    {
        [TestMethod]
        public async Task Users_HappyPath()
        {
            var apiFixture = new ApiFixture(default);
            _ = apiFixture.Start();

            var response = await apiFixture.ApiClient.GetAsync("users");

            Assert.IsTrue(response.IsSuccessStatusCode);
        }
    }
}