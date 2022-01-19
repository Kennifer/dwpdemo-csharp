using System;
using System.Net;
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
        public async Task GetAsync_HappyPath()
        {
            const string expectedUrl = "/is/there/anyone/out/there";
            const string expectedBody = "It works!";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            var wireMockServer = WireMockServer.Start();

            wireMockServer.Given(Request.Create().WithPath(expectedUrl))
                .RespondWith(
                    Response.Create().WithSuccess().WithBody(expectedBody));

            var httpClient = new System.Net.Http.HttpClient() { BaseAddress = new Uri(wireMockServer.Urls[0]) };

            var sut = new HttpClientProxy(httpClient);

            var response = await sut.GetAsync(expectedUrl);

            Assert.That(response, Is.Not.Null);
            Assert.That(response.StatusCode, Is.EqualTo(expectedStatusCode));
            var body = await response.Content.ReadAsStringAsync();
            Assert.That(body, Is.EqualTo(expectedBody));
        }
    }
}