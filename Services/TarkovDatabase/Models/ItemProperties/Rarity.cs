using System.Text.Json.Serialization;

namespace TarkovItemBot.Services
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Rarity
    {
        None,
        Common,
        Rare,
        SuperRare
    }
}
