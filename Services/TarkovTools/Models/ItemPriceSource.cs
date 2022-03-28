using System.Text.Json.Serialization;

namespace TarkovItemBot.Services.TarkovTools
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ItemPriceSource
    {
        Prapor,
        Therapist,
        Fence,
        Skier,
        Peacekeeper,
        Mechanic,
        Ragman,
        Jaeger,
        FleaMarket
    }
}
