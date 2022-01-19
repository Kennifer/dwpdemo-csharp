using System;
using System.Net.Http;
using DWP.Demo.Api.Domain.Filters;
using DWP.Demo.Api.Domain.GeoSpatial;
using DWP.Demo.Api.Domain.GeoSpatial.Impelentation;
using DWP.Demo.Api.Domain.HttpClient;
using DWP.Demo.Api.Domain.HttpClient.Implementation;
using DWP.Demo.Api.Domain.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace DWP.Demo.Api.Configuration
{
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