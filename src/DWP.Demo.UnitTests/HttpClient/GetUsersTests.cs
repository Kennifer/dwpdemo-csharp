using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DWP.Demo.Api.HttpClient;
using DWP.Demo.Api.Types;
using Moq;
using NUnit.Framework;

namespace DWP.Demo.UnitTests.HttpClient
{
    [TestFixture]
    public class GetUsersTests
    {
        [Test]
        public async Task Execute_ReturnsListOfUsers()
        {
            const string url = "/users";

            var sut = new GetUsers();

            var users = await sut.Execute();
            
            Assert.That(users, Is.InstanceOf<IEnumerable<User>>());

            Assert.That(users, Is.Not.Null);
            Assert.That(users, Is.Empty);
        }

        [Test]
        public async Task Execute_ManyResults_ReturnsUsers()
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

            var sut = new GetUsers();

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
    }

    public interface IGetUsers
    {
        Task<IEnumerable<User>> Execute();
    }
    
    public class GetUsers : IGetUsers
    {
        public async Task<IEnumerable<User>> Execute()
        {
            return Enumerable.Empty<User>();
        }
    }
}