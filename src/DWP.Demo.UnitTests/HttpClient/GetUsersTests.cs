using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DWP.Demo.Api.Types;
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