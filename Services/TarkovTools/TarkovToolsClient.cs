using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TarkovItemBot.Helpers;
using TarkovItemBot.Options;

namespace TarkovItemBot.Services.TarkovTools
{
    public class TarkovToolsClient
    {
        private readonly HttpClient _httpClient;

        public TarkovToolsClient(HttpClient httpClient, IOptions<TarkovToolsOptions> config)
        {
            Console.WriteLine(config.Value.BaseUri);
            httpClient.BaseAddress = new Uri(config.Value.BaseUri);
            httpClient.DefaultRequestHeaders.Add("User-Agent",
                $"TarkovItemBot/{AssemblyHelper.GetInformationalVersion()}");

            _httpClient = httpClient;
        }

        private record Response(Data Data);
        private record Data(ItemPriceData Item);

        public async Task<ItemPriceData> GetItemPriceDataAsync(string id)
        {
            var queryObject = new Dictionary<string, string>()
            {
                {"query",
                    $"query {{ item(id: \"{id}\")" + 
                    "{ id avg24hPrice changeLast48h low24hPrice high24hPrice sellFor { price source currency } } }" }
            };

            var request = await _httpClient.PostAsJsonAsync("", queryObject);
            var response = await request.Content.ReadFromJsonAsync<Response>();

            return response.Data.Item;
        }
    }
}
