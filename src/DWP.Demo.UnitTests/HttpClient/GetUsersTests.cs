using System.Collections.Generic;
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
        }
    }

    public interface IGetUsers
    {
        Task<IEnumerable<User>> Execute();
    }
    
    public class GetUsers : IGetUsers
    {
        public Task<IEnumerable<User>> Execute()
        {
            throw new System.NotImplementedException();
        }
    }
}