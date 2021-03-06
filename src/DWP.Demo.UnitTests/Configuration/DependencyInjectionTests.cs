using DWP.Demo.Api.Configuration;
using DWP.Demo.Api.Controllers;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DWP.Demo.UnitTests.Configuration
{
    [TestFixture]
    public class DependencyInjectionTests
    {
        [Test]
        public void AddConfiguration_HappyPath()
        {
            var services = new ServiceCollection();

            services.AddSingleton<UserController>();
            services.AddDWPDemoApiDomain("http://localhost");

            var serviceProvider = services.BuildServiceProvider();

            var controllerInstance = serviceProvider.GetService<UserController>();

            Assert.That(controllerInstance, Is.Not.Null);
        }
    }
}