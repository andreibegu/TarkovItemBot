using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TarkovItemBot.Helpers;
using TarkovItemBot.Options;

namespace TarkovItemBot.Services.TarkovDatabaseSearch
{
    public class TarkovSearchAuthClient
    {
        private record TokenResponse(string Token, int Expires);

        private readonly HttpClient _httpClient;

        public TarkovSearchAuthClient(HttpClient httpClient, IOptions<TarkovDatabaseOptions> config)
        {
            httpClient.BaseAddress = new Uri(config.Value.SearchBaseUri);
            httpClient.DefaultRequestHeaders.Add("User-Agent",
                $"TarkovItemBot/{AssemblyHelper.GetInformationalVersion()}");
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", config.Value.SearchToken);

            _httpClient = httpClient;
        }

        public async Task<string> GetTokenAsync()
            => (await _httpClient.GetFromJsonAsync<TokenResponse>("token")).Token;
    }
}
