using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TarkovItemBot.Services.TarkovTools
{
    public class ItemPriceData
    {
        public string Id { get; set; }
        public int? Avg24hPrice { get; set; }
        public double? ChangeLast48h { get; set; }
        public int? Low24hPrice { get; set; }
        public int? High24hPrice { get; set; }
        [JsonPropertyName("sellFor")]
        public ItemSellData[] SellData { get; set; }
    }
}
