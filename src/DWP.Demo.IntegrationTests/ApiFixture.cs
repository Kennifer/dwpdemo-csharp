using System;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using DWP.Demo.Api;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace DWP.Demo.IntegrationTests
{
    public class ApiFixture
    {
        public HttpClient ApiClient { get; }
            
        private readonly IWebHostBuilder _applicationFixture;

        public ApiFixture(IConfiguration configuration = default)
        {
            var address = $"http://localhost:{GetFreePort()}";
                
            _applicationFixture =
                Program.CreateHostBuilder(default)
                    .UseKestrel()
                    .UseUrls(address)
                    .ConfigureAppConfiguration((context, builder) =>
                    {
                        if (configuration is object)
                        {
                            builder.AddConfiguration(configuration);
                        }
                    });
                
            ApiClient = new HttpClient()
            {
                BaseAddress = new Uri(address)
            };
        }
            
        public Task Start() =>
            _applicationFixture.Build().StartAsync();
                
        private static int GetFreePort()
        {
            int port;
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, 0);
                socket.Bind(localEndPoint);
                localEndPoint = (IPEndPoint)socket.LocalEndPoint;
                port = localEndPoint.Port;
            }
            finally
            {
                socket.Close();
            }
            return port;
        }
    }
}