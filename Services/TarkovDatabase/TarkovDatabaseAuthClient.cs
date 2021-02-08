using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection;
using System.Threading.Tasks;
using TarkovItemBot.Options;

namespace TarkovItemBot.Services
{
    public class TarkovDatabaseAuthClient
    {
        private record TokenResponse(string Token);

        private readonly HttpClient _httpClient;

        public TarkovDatabaseAuthClient(HttpClient httpClient, IOptions<TarkovDatabaseOptions> config)
        {
            httpClient.BaseAddress = new Uri(config.Value.BaseUri);
            httpClient.DefaultRequestHeaders.Add("User-Agent",
                $"TarkovItemBot/{Assembly.GetEntryAssembly().GetName().Version.ToString(3)}");
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", config.Value.Token);

            _httpClient = httpClient;
        }

        public async Task<string> GetTokenAsync()
            => (await _httpClient.GetFromJsonAsync<TokenResponse>("token")).Token;
    }
}
