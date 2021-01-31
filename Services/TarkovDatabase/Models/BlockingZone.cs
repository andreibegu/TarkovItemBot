using System.Text.Json.Serialization;

namespace TarkovItemBot.Services
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum BlockingZone
    {
        Earpiece,
        Eyewear,
        Headwear,
        FaceCover
    }
}
