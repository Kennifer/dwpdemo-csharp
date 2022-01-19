using System.Net.Http;
using System.Threading.Tasks;

namespace DWP.Demo.Api.Domain.HttpClient
{
    public class HttpClientProxy : IHttpClient
    {
        private readonly System.Net.Http.HttpClient _httpClient;

        public HttpClientProxy(System.Net.Http.HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<HttpResponseMessage> GetAsync(string url)
            => _httpClient.GetAsync(url);
    }
}