
using System.Text.Json.Serialization;

namespace TarkovItemBot.Services
{
    [Kind(ItemKind.Common)]
    public class CommonItem
    {
        [JsonPropertyName("_id")]
        public string Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public float Weight { get; set; }
        public int MaxStack { get; set; }
        public string Rarity { get; set; }
        public Grid Grid { get; set; }
        [JsonPropertyName("_modified")]
        public int Modified { get; set; }
        [JsonPropertyName("_kind")]
        public ItemKind Kind { get; set; }
    }
}
