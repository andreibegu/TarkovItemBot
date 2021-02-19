using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TarkovItemBot.Helpers;
using TarkovItemBot.Options;

namespace TarkovItemBot.Services
{
    public class TarkovSearchClient
    {
        private readonly HttpClient _httpClient;
        public TarkovSearchClient(HttpClient httpClient, IOptions<TarkovDatabaseOptions> config)
        {
            httpClient.BaseAddress = new Uri(config.Value.SearchBaseUri);
            httpClient.DefaultRequestHeaders.Add("User-Agent",
                $"TarkovItemBot/{AssemblyHelper.GetInformationalVersion()}");

            _httpClient = httpClient;
        }

        private record SearchResult(int Count, IReadOnlyCollection<SearchItem> Data);

        public async Task<IReadOnlyCollection<SearchItem>> SearchAsync(string query, int limit = 30)
        {
            var uriQuery = new Dictionary<string, object>
            {
                ["query"] = query,
                ["limit"] = limit,
            };

            var response = await _httpClient.GetFromJsonAsync<SearchResult>("search" + uriQuery.AsQueryString());

            return response.Data;
        }
    }
}
