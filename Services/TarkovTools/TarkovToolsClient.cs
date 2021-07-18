using Microsoft.Extensions.Options;
using System;
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
            httpClient.BaseAddress = new Uri(config.Value.BaseUri);
            httpClient.DefaultRequestHeaders.Add("User-Agent",
                $"TarkovItemBot/{AssemblyHelper.GetInformationalVersion()}");

            _httpClient = httpClient;
        }

        private record Response(Data Data);
        private record Data(ItemPriceData Item);

        public async Task<ItemPriceData> GetItemPriceDataAsync(string id)
        {
            var query = new
            {
                Query = $@"
                query Query {{
                    item(id: ""{id}"") {{
                        id
                        avg24hPrice
                        changeLast48h
                        low24hPrice
                        high24hPrice
                    }}
                }}",
            };

            var request = await _httpClient.PostAsJsonAsync("", query);
            var response = await request.Content.ReadFromJsonAsync<Response>();

            return response.Data.Item;
        }
    }
}
