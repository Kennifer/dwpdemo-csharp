using System;
using System.Net.Http;
using DWP.Demo.Api.Controllers;
using DWP.Demo.Api.Domain.Filters;
using DWP.Demo.Api.Domain.GeoSpatial;
using DWP.Demo.Api.Domain.GeoSpatial.Impelentation;
using DWP.Demo.Api.Domain.HttpClient;
using DWP.Demo.Api.Domain.HttpClient.Implementation;
using DWP.Demo.Api.Domain.Logging;
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
    
    public static class DependencyInjectionConfiguration {

        public static IServiceCollection AddDWPDemoApiDomain(this IServiceCollection serviceCollection, string baseUrl)
        {
            serviceCollection.AddSingleton<IGetUsers, GetUsers>();
            serviceCollection.AddSingleton<IGetUsersByCity, GetUsersByCity>();
            serviceCollection.AddSingleton<IDistanceCalculator, DistanceCalculator>();
            serviceCollection.AddSingleton<IUserDistanceFilter, UserDistanceFilter>();
            serviceCollection.AddSingleton<ILogger, LoggerStub>();

            serviceCollection.AddSingleton<IHttpClient, HttpClientProxy>();
            serviceCollection.AddSingleton<HttpClient>(new HttpClient()
            {
                BaseAddress = new Uri(baseUrl)
            });
            
            return serviceCollection;
        }

        public class LoggerStub : ILogger
        {
            public void LogWarning(string message, params object[] paramsValues)
            {
                
            }
        }
    
    }
}