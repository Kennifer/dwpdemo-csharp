using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DWP.Demo.Api.Logging;
using DWP.Demo.Api.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DWP.Demo.Api.HttpClient.Implementation
{
    public class GetUsers : IGetUsers
    {
        private readonly IHttpClient _httpClient;
        private readonly ILogger _logger;

        private readonly string _url = "/users";

        public GetUsers(IHttpClient httpClient, ILogger logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<IEnumerable<User>> Execute()
        {
            var response = await _httpClient.GetAsync(_url);

            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();

                return MapOut(body);
            }

            _logger.LogWarning("Request to {url} failed with status code {statuscode}", _url, response.StatusCode);
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
    }
}