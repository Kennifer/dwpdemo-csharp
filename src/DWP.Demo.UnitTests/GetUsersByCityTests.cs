using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            
            var sut = new GetUsersByCity();

            var result = await sut.Execute(city);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<IEnumerable<User>>());
        }
    }

    public interface IGetUsersByCity
    {
        Task<IEnumerable<User>> Execute(string city);
    }

    public class User
    {
    }

    public class GetUsersByCity : IGetUsersByCity
    {
        public async Task<IEnumerable<User>> Execute(string city)
        {
            return Enumerable.Empty<User>();
        }
    }
}