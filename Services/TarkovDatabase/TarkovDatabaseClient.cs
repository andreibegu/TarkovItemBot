using Discord.Addons.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using TarkovItemBot.Helpers;

namespace TarkovItemBot.Services
{
    public class TarkovDatabaseClient
    {
        private readonly ConcurrentDictionary<ItemKind, Type> _kindMap = new ConcurrentDictionary<ItemKind, Type>();
        private readonly HttpClient _httpClient;
        public TarkovDatabaseClient(HttpClient httpClient, IConfiguration config)
        {
            BuildKindMap();

            httpClient.BaseAddress = new Uri(config["TarkovDatabase:BaseUri"]);
            httpClient.DefaultRequestHeaders.Add("User-Agent",
                $"TarkovItemBot/{Assembly.GetEntryAssembly().GetName().Version}");

            _httpClient = httpClient;
        }

        private void BuildKindMap()
        {
            var fields = Enum.GetNames(typeof(ItemKind)).Select(x => typeof(ItemKind).GetField(x));

            foreach (var field in fields)
                _kindMap.TryAdd((ItemKind)field.GetRawConstantValue(), field.GetCustomAttribute<KindTypeAttribute>()?.KindType);
        }

        public Task<ItemsInfo> GetItemsInfoAsync()
            => _httpClient.GetFromJsonAsync<ItemsInfo>("item");

        public async Task<T> GetItemAsync<T>(string id) where T : CommonItem
        {
            var kind = _kindMap.FirstOrDefault(x => x.Value == typeof(T)).Key;
            var response = await _httpClient.GetFromJsonAsync<T>($"item/{kind.ToString().ToCamelCase()}/{id}");
            return response;
        }

        public async Task<IEmbedableItem> GetEmbedableItemAsync(string id, ItemKind kind)
        {
            var kindType = _kindMap[kind];
            var response = await _httpClient.GetFromJsonAsync($"item/{kind.ToString().ToCamelCase()}/{id}", kindType);
            return response as IEmbedableItem;
        }

        private record ItemResponse<T>(int Total, List<T> Items) where T : CommonItem;

        public async Task<List<T>> GetItemsAsync<T>() where T : CommonItem
        {
            var kind = _kindMap.FirstOrDefault(x => x.Value == typeof(T)).Key;

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
        
        private record LocationResponse(int Total, List<Location> Items);

        public async Task<List<Location>> GetLocationsAsync(string text = null, int limit = 15)
        {
            var query = new Dictionary<string, object>();

            if (text != null) query["text"] = text;
            query["limit"] = limit;

            var response = await _httpClient.GetFromJsonAsync<LocationResponse>("location" + query.AsQueryString());
            return response.Items;
        }
    }
}
