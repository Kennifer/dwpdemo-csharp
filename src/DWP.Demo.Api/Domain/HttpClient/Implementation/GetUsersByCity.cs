using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DWP.Demo.Api.Domain.Logging;
using DWP.Demo.Api.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DWP.Demo.Api.Domain.HttpClient.Implementation
{
    public class GetUsersByCity : IGetUsersByCity
    {
        private readonly IHttpClient _httpClient;
        private readonly ILogger _logger;

        public GetUsersByCity(IHttpClient httpClient, ILogger logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<IEnumerable<User>> Execute(string city)
        {
            var url = BuildUrl(city);

            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                return MapOut(body);
            }
            
            _logger.LogWarning("Request to {url} failed with status code {statuscode}", url, response.StatusCode);
            return Enumerable.Empty<User>();
        }

        private IEnumerable<User> MapOut(string body)
            => JsonConvert.DeserializeObject<IEnumerable<User>>(body, new JsonSerializerSettings()
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            });

        private string BuildUrl(string city)
            => $"/city/{city}/users";
    }
}