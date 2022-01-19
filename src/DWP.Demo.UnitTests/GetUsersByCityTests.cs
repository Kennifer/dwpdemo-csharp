using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;

namespace DWP.Demo.UnitTests
{
    [TestFixture]
    public class GetUsersByCityTests
    {
        [Test]
        public async Task Execute_ReturnsUsers()
        {
            const string city = "some city";
            
            var sut = new GetUsersByCity(Mock.Of<IHttpProxy>());

            var result = await sut.Execute(city);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<IEnumerable<User>>());
        }

        [Test]
        public async Task Execute_MakesRequest()
        {
            const string city = "manchester";
            const string expectedRequestAddress = "/city/manchester/users";

            var httpProxy = new Mock<IHttpProxy>();

            var sut = new GetUsersByCity(httpProxy.Object);

            _ = await sut.Execute(city);

            httpProxy.Verify(x => x.GetAsync(expectedRequestAddress));
        }
    }

    public interface IGetUsersByCity
    {
        Task<IEnumerable<User>> Execute(string city);
    }

    public interface IHttpProxy
    {
        Task GetAsync(string url);
    }

    public class User
    {
    }

    public class GetUsersByCity : IGetUsersByCity
    {
        private readonly IHttpProxy _httpProxy;

        public GetUsersByCity(IHttpProxy httpProxy)
        {
            _httpProxy = httpProxy;
        }

        public async Task<IEnumerable<User>> Execute(string city)
        {
            var url = BuildUrl(city);

            await _httpProxy.GetAsync(url);
            return Enumerable.Empty<User>();
        }

        private string BuildUrl(string city)
            => $"/city/{city}/users";
    }
}