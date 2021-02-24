using System.Text.Json.Serialization;

namespace TarkovItemBot.Services.TarkovDatabase
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
