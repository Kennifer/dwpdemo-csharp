using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DWP.Demo.Api.HttpClient;
using DWP.Demo.Api.Logging;
using DWP.Demo.Api.Types;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;

namespace DWP.Demo.UnitTests.HttpClient
{
    [TestFixture]
    public class GetUsersTests
    {
        [Test]
        public async Task Execute_NoUsers_ReturnsEmptyList()
        {
            const string requestUrl = "/users";
            const string responseString = "[ ]";

            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(responseString, Encoding.UTF8, "application/json")
            };
            
            var httpClient = new Mock<IHttpClient>();
            httpClient.Setup(x => x.GetAsync(requestUrl))
                .ReturnsAsync(httpResponse);

            var sut = new GetUsers(httpClient.Object, Mock.Of<ILogger>());

            var users = await sut.Execute();
            
            Assert.That(users, Is.InstanceOf<IEnumerable<User>>());

            Assert.That(users, Is.Not.Null);
            Assert.That(users, Is.Empty);
        }

        [Test]
        public async Task Execute_ManyUsers_ReturnsUsers()
        {
            const string requestUrl = "/users";
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

            var sut = new GetUsers(httpClient.Object, Mock.Of<ILogger>());

            var users = await sut.Execute();

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

        [TestCase(HttpStatusCode.BadRequest)]
        [TestCase(HttpStatusCode.InternalServerError)]
        public async Task Execute_InvalidHttpResponse_ReturnsEmptyList(HttpStatusCode status)
        {
            const string requestUrl = "/users";
            
            var httpResponse = new HttpResponseMessage(status);
            
            var httpClient = new Mock<IHttpClient>();
            httpClient.Setup(x => x.GetAsync(requestUrl))
                .ReturnsAsync(httpResponse);

            var sut = new GetUsers(httpClient.Object, Mock.Of<ILogger>());

            var users = await sut.Execute();

            Assert.That(users, Is.Not.Null);
            Assert.That(users, Is.Empty);
        }
        
        [Test]
        public async Task Execute_InvalidHttpResponse_LogsError()
        {
            const string expectedUrl = "/users";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.BadRequest; 
            
            var httpResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
            
            var httpClient = new Mock<IHttpClient>();
            httpClient.Setup(x => x.GetAsync(expectedUrl))
                .ReturnsAsync(httpResponse);

            var logger = new Mock<ILogger>();
            
            var sut = new GetUsers(httpClient.Object, logger.Object);

            _ = await sut.Execute();

            logger.Verify(x => x.LogWarning(It.IsAny<string>(), expectedUrl, expectedStatusCode));
        }
    }

    public interface IGetUsers
    {
        Task<IEnumerable<User>> Execute();
    }
    
    public class GetUsers : IGetUsers
    {
        private readonly IHttpClient _httpClient;

        private readonly string _url = "/users";

        public GetUsers(IHttpClient httpClient, ILogger logger)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<User>> Execute()
        {
            var response = await _httpClient.GetAsync(_url);

            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();

                return MapOut(body);
            }

            return Enumerable.Empty<User>();
        }

        private IEnumerable<User> MapOut(string body)
            => JsonConvert.DeserializeObject<IEnumerable<User>>(body, new JsonSerializerSettings()
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            });
    }
}