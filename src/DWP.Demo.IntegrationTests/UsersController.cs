using System.Threading.Tasks;
using NUnit.Framework;

namespace DWP.Demo.IntegrationTests
{
    [TestFixture]
    public class UsersController
    {
        [Test]
        public async Task Users_HappyPath()
        {
            var apiFixture = new ApiFixture(default);
            _ = apiFixture.Start();

            var response = await apiFixture.ApiClient.GetAsync("users");

            Assert.IsTrue(response.IsSuccessStatusCode);
        }
    }
}