using System.Net.Http;
using System.Threading.Tasks;

namespace DWP.Demo.Api.Domain.HttpClient
{
    public interface IHttpClient
    {
        Task<HttpResponseMessage> GetAsync(string url);
    }
}