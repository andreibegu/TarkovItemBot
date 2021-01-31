using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TarkovItemBot.Helpers;

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

        public async Task<T> GetItemAsync<T>(string id) where T : CommonItem
        {
            var kind = ((KindAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(KindAttribute))).Kind;
            var response = await _httpClient.GetFromJsonAsync<T>($"item/{kind.ToString().ToCamelCase()}/{id}");
            return response;
        }

        private record ItemResponse<T>(int Total, List<T> Items) where T : CommonItem;

        public async Task<List<T>> GetItemsAsync<T>() where T : CommonItem
        {
            var kind = ((KindAttribute) Attribute.GetCustomAttribute(typeof(T), typeof(KindAttribute))).Kind;

            var index = await GetItemsInfoAsync();
            var total = index.Kinds[kind].Count;

            int limit = 100;
            var pages = total%limit == 0 ? total/limit : total/limit + 1;

            var items = new List<T>();
            for(int i = 0; i < pages; i++)
            {
                var offset = limit * i;

                var response = await _httpClient.GetFromJsonAsync<ItemResponse<T>>
                    ($"item/{kind.ToString().ToCamelCase()}?limit={limit}&offset={offset}");
                items.AddRange(response.Items);
            }

            return items;
        }
    }
}
