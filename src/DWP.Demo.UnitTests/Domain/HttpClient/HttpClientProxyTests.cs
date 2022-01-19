using System;
using System.Net.Http;
using System.Threading.Tasks;
using DWP.Demo.Api.Domain.HttpClient;
using NUnit.Framework;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace DWP.Demo.UnitTests.Domain.HttpClient
{
    [TestFixture]
    public class HttpClientProxyTests
    {
        [Test]
        public async Task Test_Test_Test()
        {
            const string expectedUrl = "/is/there/anyone/out/there";
            const string expectedBody = "It works!";
            const int expectedStatusCode = 200;
            var wireMockServer = WireMockServer.Start();

            wireMockServer.Given(Request.Create().WithPath(expectedUrl))
                .RespondWith(
                    Response.Create().WithStatusCode(expectedStatusCode).WithBody(expectedBody));

            var httpClient = new System.Net.Http.HttpClient() { BaseAddress = new Uri(wireMockServer.Urls[0]) };

            var sut = new HttpClientProxy(httpClient);

            var response = await sut.GetAsync(expectedUrl);

            Assert.That(response, Is.Not.Null);
            Assert.That(response.StatusCode, Is.EqualTo(expectedStatusCode));
            var body = response.Content.ReadAsStringAsync();
            Assert.That(body, Is.EqualTo(expectedBody));
        }
    }
    
    public class HttpClientProxy : IHttpClient
    {
        private readonly System.Net.Http.HttpClient _httpClient;

        public HttpClientProxy(System.Net.Http.HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<HttpResponseMessage> GetAsync(string url)
        {
            throw new System.NotImplementedException();
        }
    }
}