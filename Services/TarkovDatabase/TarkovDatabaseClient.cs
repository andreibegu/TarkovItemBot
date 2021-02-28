using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Threading.Tasks;
using TarkovItemBot.Helpers;
using TarkovItemBot.Options;

namespace TarkovItemBot.Services.TarkovDatabase
{
    public class TarkovDatabaseClient
    {
        private readonly ConcurrentDictionary<ItemKind, Type> _kindMap = new ConcurrentDictionary<ItemKind, Type>();
        private readonly HttpClient _httpClient;
        public TarkovDatabaseClient(HttpClient httpClient, IOptions<TarkovDatabaseOptions> config)
        {
            BuildKindMap();

            httpClient.BaseAddress = new Uri(config.Value.BaseUri);
            httpClient.DefaultRequestHeaders.Add("User-Agent",
                $"TarkovItemBot/{AssemblyHelper.GetInformationalVersion()}");

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

        public async Task<T> GetItemAsync<T>(string id) where T : BaseItem
        {
            var kind = _kindMap.FirstOrDefault(x => x.Value == typeof(T)).Key;
            var response = await _httpClient.GetFromJsonAsync<T>($"item/{kind.ToString().ToCamelCase()}/{id}");
            return response;
        }

        public async Task<IItem> GetItemAsync(string id, ItemKind kind)
        {
            var kindType = _kindMap[kind];
            var response = await _httpClient.GetFromJsonAsync($"item/{kind.ToString().ToCamelCase()}/{id}", kindType);
            return response as IItem;
        }

        private record ItemResponse<T>(int Total, IReadOnlyCollection<T> Items) where T : BaseItem;

        public async Task<IReadOnlyCollection<T>> GetItemsAsync<T>(IEnumerable<string> ids = null) where T : BaseItem
        {
            var kind = _kindMap.FirstOrDefault(x => x.Value == typeof(T)).Key;
            int total;

            if (ids == null)
            {
                var index = await GetItemsInfoAsync();
                total = index.Kinds[kind].Count;
            }
            else total = ids.Count();

            int limit = 100;
            var pages = total % limit == 0 ? total / limit : total / limit + 1;

            var items = new List<T>();
            for (int i = 0; i < pages; i++)
            {
                var offset = limit * i;

                var query = new Dictionary<string, object>()
                {
                    ["limit"] = limit,
                    ["offset"] = offset
                };

                if (ids != null)
                {
                    var end = total - offset;
                    ids = ids.Skip(offset).Take(end);
                    query["id"] = string.Join(",", ids);
                }

                var response = await _httpClient.GetFromJsonAsync<ItemResponse<T>>
                    ($"item/{kind.ToString().ToCamelCase()}{query.AsQueryString()}");
                items.AddRange(response.Items);
            }

            return items;
        }

        private record LocationResponse(int Total, IReadOnlyCollection<Location> Items);

        public async Task<IReadOnlyCollection<Location>> GetLocationsAsync(string text = null, int limit = 15)
        {
            var query = new Dictionary<string, object>();

            if (text != null) query["text"] = text;
            query["limit"] = limit;

            var response = await _httpClient.GetFromJsonAsync<LocationResponse>("location" + query.AsQueryString());
            return response.Items ?? new List<Location>();
        }
    }
}
