using System.Text.Json.Serialization;

namespace TarkovItemBot.Services
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Deafness
    {
        None,
        Low,
        Medium,
        High
    }
}
