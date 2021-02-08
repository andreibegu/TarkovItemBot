using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
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

        private record SearchResult(int Count, List<SearchItem> Data);

        public async Task<List<SearchItem>> SearchAsync(string query, int limit = 30)
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
