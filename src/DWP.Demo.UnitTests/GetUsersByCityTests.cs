using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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
            
            var sut = new GetUsersByCity(Mock.Of<IHttpClient>());

            var result = await sut.Execute(city);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<IEnumerable<User>>());
        }

        [Test]
        public async Task Execute_MakesRequest()
        {
            const string city = "manchester";
            const string expectedRequestAddress = "/city/manchester/users";

            var httpProxy = new Mock<IHttpClient>();

            var sut = new GetUsersByCity(httpProxy.Object);

            _ = await sut.Execute(city);

            httpProxy.Verify(x => x.GetAsync(expectedRequestAddress));
        }

        [Test]
        public async Task Execute_ReturnsCorrectlyPopulatedUsers()
        {
            const string city = "manchester";
            const string requestUrl = "/city/manchester/users";
            const string responseString =
                @"[
                {
                    ""id"": 1,
                    ""first_name"": ""fn1"",
                    ""last_name"": ""ln1"",
                    ""email"": ""email@1.com"",
                    ""ip_address"": ""192.168.0.1"",
                    ""latitude"": 1.1234567,
                    ""longitude"": -1.1234567
                },
                {
                    ""id"": 2,
                    ""first_name"": ""fn2"",
                    ""last_name"": ""ln2"",
                    ""email"": ""email@2.com"",
                    ""ip_address"": ""192.168.0.2"",
                    ""latitude"": 2.1234567,
                    ""longitude"": -2.1234567
                }
            ]";

            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(responseString, Encoding.UTF8, "application/json")
            };
            
            var httpClient = new Mock<IHttpClient>();
            httpClient.Setup(x => x.GetAsync(requestUrl))
                .ReturnsAsync(httpResponse);

            var sut = new GetUsersByCity(httpClient.Object);

            var users = await sut.Execute(city);

            Assert.That(users, Is.Not.Null);
            Assert.That(users.Count(), Is.EqualTo(2));

            var user1 = users.SingleOrDefault(x => x.Id == 1);
            Assert.That(user1, Is.Not.Null);
            Assert.That(user1.FirstName, Is.EqualTo("fn1"));
            Assert.That(user1.LastName, Is.EqualTo("ln1"));
            Assert.That(user1.Email, Is.EqualTo("email@1.com"));
            Assert.That(user1.IpAddress, Is.EqualTo("192.168.0.1"));
            Assert.That(user1.Latitude, Is.EqualTo(1.1234567));
            Assert.That(user1.Longitude, Is.EqualTo(-1.1234567));
            
            var user2 = users.SingleOrDefault(x => x.Id == 2);
            Assert.That(user2, Is.Not.Null);
            Assert.That(user2.FirstName, Is.EqualTo("fn2"));
            Assert.That(user2.LastName, Is.EqualTo("ln2"));
            Assert.That(user2.Email, Is.EqualTo("email@2.com"));
            Assert.That(user2.IpAddress, Is.EqualTo("192.168.0.2"));
            Assert.That(user2.Latitude, Is.EqualTo(2.1234567));
            Assert.That(user2.Longitude, Is.EqualTo(-2.1234567));
        }
    }

    public interface IGetUsersByCity
    {
        Task<IEnumerable<User>> Execute(string city);
    }

    public interface IHttpClient
    {
        Task<HttpResponseMessage> GetAsync(string url);
    }

    public record User
    {
        public int Id { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string Email { get; init; }
        public string IpAddress { get; init; }
        public decimal Latitude { get; init; }
        public decimal Longitude { get; init; }
    }

    public class GetUsersByCity : IGetUsersByCity
    {
        private readonly IHttpClient _httpClient;

        public GetUsersByCity(IHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<User>> Execute(string city)
        {
            var url = BuildUrl(city);

            var response = await _httpClient.GetAsync(url);
            var body = await response.Content.ReadAsStringAsync();

            return MapOut(body);
        }

        private IEnumerable<User> MapOut(string body)
            => JsonConvert.DeserializeObject<IEnumerable<User>>(body, new JsonSerializerSettings()
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            });

        private string BuildUrl(string city)
            => $"/city/{city}/users";
    }
}