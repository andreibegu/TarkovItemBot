using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Threading.Tasks;

namespace TarkovItemBot.Services
{
    public class TarkovDatabaseClient
    {
        private readonly HttpClient _httpClient;
        public TarkovDatabaseClient(HttpClient httpClient, IConfiguration config)
        {
            httpClient.BaseAddress = new Uri(config["TarkovDatabase:BaseUri"]);
            httpClient.DefaultRequestHeaders.Add("User-Agent",
                $"TarkovItemBot/{Assembly.GetEntryAssembly().GetName().Version}");

            _httpClient = httpClient;
        }

        public Task<ItemsInfo> GetItemsInfoAsync()
            => _httpClient.GetFromJsonAsync<ItemsInfo>("item");
    }
}
