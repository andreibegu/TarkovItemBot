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
        private const int PageLimit = 100;

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

        public Task<ItemIndex> GetItemIndexAsync()
            => _httpClient.GetFromJsonAsync<ItemIndex>("item");

        public async Task<T> GetItemAsync<T>(string id) where T : IItem
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

        private record Response<T>(int Total, IReadOnlyCollection<T> Items);

        private async Task<IReadOnlyCollection<T>> GetItemsAsync<T>(ItemKind kind, Dictionary<string, object> query)
            where T : IItem
        {
            // Unsafe workaround for no interface deserialization
            string path = $"item/{kind.ToString().ToCamelCase()}{query.AsQueryString()}";

            IReadOnlyCollection<T> response;
            if (typeof(T) != typeof(IItem))
            {
                var result = await _httpClient.GetFromJsonAsync<Response<T>>(path);
                response = result.Items;
            }
            else
            {
                var kindType = _kindMap[kind];
                var responseType = typeof(Response<>).MakeGenericType(kindType);
                dynamic result = await _httpClient.GetFromJsonAsync(path, responseType);
                response = result.Items;
            }

            return response;
        }

        private async Task<IReadOnlyCollection<T>> GetItemsByCountAsync<T>(ItemKind kind, int count) where T : IItem
        {
            var pages = count % PageLimit == 0 ? count / PageLimit : count / PageLimit + 1;
            var items = new List<T>();

            for (int i = 0; i < pages; i++)
            {
                var offset = PageLimit * i;
                var query = new Dictionary<string, object>()
                {
                    ["limit"] = PageLimit,
                    ["offset"] = offset
                };

                items.AddRange(await GetItemsAsync<T>(kind, query));
            }

            return items;
        }

        private async Task<IReadOnlyCollection<T>> GetItemsByIdsAsync<T>(ItemKind kind, IEnumerable<string> ids) where T : IItem
        {
            var query = new Dictionary<string, object>()
            {
                ["limit"] = PageLimit
            };

            var items = new List<T>();
            var queries = EnumerableHelper.ToQueryStrings(ids, PageLimit);
            foreach (var idQuery in queries)
            {
                query["id"] = idQuery;

                items.AddRange(await GetItemsAsync<T>(kind, query));
            }

            return items;
        }

        public async Task<IReadOnlyCollection<T>> GetItemsAsync<T>() where T : IItem
        {
            var kind = _kindMap.FirstOrDefault(x => x.Value == typeof(T)).Key;
            var index = await GetItemIndexAsync();
            var count = index.Kinds[kind].Count;
            return await GetItemsByCountAsync<T>(kind, count);
        }

        public async Task<IReadOnlyCollection<T>> GetItemsAsync<T>(IEnumerable<string> ids) where T : IItem
        {
            var kind = _kindMap.FirstOrDefault(x => x.Value == typeof(T)).Key;
            return await GetItemsByIdsAsync<T>(kind, ids);
        }

        public async Task<IReadOnlyCollection<IItem>> GetItemsAsync(ItemKind kind)
        {
            var index = await GetItemIndexAsync();
            var count = index.Kinds[kind].Count;
            return await GetItemsByCountAsync<IItem>(kind, count);
        }

        public Task<IReadOnlyCollection<IItem>> GetItemsAsync(ItemKind kind, IEnumerable<string> ids)
            => GetItemsByIdsAsync<IItem>(kind, ids);

        public Task<Location> GetLocationAsync(string id)
            => _httpClient.GetFromJsonAsync<Location>($"location/{id}");

        public async Task<IReadOnlyCollection<Location>> GetLocationsAsync(int limit = 100, string text = null)
        {
            var query = new Dictionary<string, object>()
            {
                ["limit"] = limit
            };

            if (text != null) query["text"] = text;

            var response = await _httpClient.GetFromJsonAsync<Response<Location>>("location" + query.AsQueryString());
            return response.Items ?? new List<Location>();
        }

        public Task<Module> GetModuleAsync(string id)
            => _httpClient.GetFromJsonAsync<Module>($"hideout/module/{id}");

        private Task<Response<Module>> GetModulesAsync(Dictionary<string, object> query)
            =>  _httpClient.GetFromJsonAsync<Response<Module>>("hideout/module" + query.AsQueryString());

        public async Task<IReadOnlyCollection<Module>> GetModulesAsync(int count = 100, string text = null, string material = null)
        {
            var query = new Dictionary<string, object>()
            {
                ["limit"] = PageLimit
            };

            if (text != null) query["text"] = text;
            if (material != null) query["material"] = material;

            var pages = count % PageLimit == 0 ? count / PageLimit : count / PageLimit + 1;
            var items = new List<Module>();

            for (int i = 0; i < pages; i++)
            {
                if (i == pages - 1) query["limit"] = count;
                var offset = PageLimit * i;
                query["offset"] = offset;

                items.AddRange((await GetModulesAsync(query)).Items);
            }

            return items ?? new List<Module>();
        }

        public async Task<IReadOnlyCollection<Module>> GetModulesAsync(IEnumerable<string> ids, string text = null, string material = null)
        {
            var query = new Dictionary<string, object>()
            {
                ["limit"] = PageLimit
            };

            if (text != null) query["text"] = text;
            if (material != null) query["material"] = material;

            var queries = EnumerableHelper.ToQueryStrings(ids, PageLimit);
            var items = new List<Module>();

            foreach (var idQuery in queries)
            {
                query["id"] = idQuery;
                items.AddRange((await GetModulesAsync(query)).Items);
            }

            return items ?? new List<Module>();
        }

        public Task<Production> GetProductionAsync(string id)
            => _httpClient.GetFromJsonAsync<Production>($"hideout/production/{id}");

        private Task<Response<Production>> GetProductionsAsync(Dictionary<string, object> query)
            => _httpClient.GetFromJsonAsync<Response<Production>>("hideout/production" + query.AsQueryString());

        public async Task<IReadOnlyCollection<Production>> GetProductionsAsync(int count = 100, string module = null, 
            string material = null, string outcome = null)
        {
            var query = new Dictionary<string, object>();

            if (module != null) query["module"] = module;
            if (material != null) query["material"] = material;
            if (outcome != null) query["outcome"] = outcome;

            var pages = count % PageLimit == 0 ? count / PageLimit : count / PageLimit + 1;
            var items = new List<Production>();

            for (int i = 0; i < pages; i++)
            {
                if (i == pages - 1) query["limit"] = count;
                var offset = PageLimit * i;
                query["offset"] = offset;

                items.AddRange((await GetProductionsAsync(query)).Items);
            }

            return items ?? new List<Production>();
        }

        public async Task<IReadOnlyCollection<Production>> GetProductionsAsync(IEnumerable<string> ids, string module = null,
            string material = null, string outcome = null)
        {
            var query = new Dictionary<string, object>()
            {
                ["limit"] = PageLimit
            };

            if (module != null) query["module"] = module;
            if (material != null) query["material"] = material;
            if (outcome != null) query["outcome"] = outcome;

            var queries = EnumerableHelper.ToQueryStrings(ids, PageLimit);
            var items = new List<Production>();

            foreach (var idQuery in queries)
            {
                query["id"] = idQuery;
                items.AddRange((await GetProductionsAsync(query)).Items);
            }

            return items ?? new List<Production>();
        }
    }
}
