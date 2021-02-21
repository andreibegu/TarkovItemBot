using System.Text.Json.Serialization;

namespace TarkovItemBot.Services
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum RicochetChance
    {
        None,
        Low,
        Medium,
        High
    }
}
