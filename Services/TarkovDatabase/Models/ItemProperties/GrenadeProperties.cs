using System.Text.Json.Serialization;

namespace TarkovItemBot.Services
{
    public class GrenadeProperties
    {
        public float Delay { get; set; }
        [JsonPropertyName("fragCount")]
        public float FragmentCount { get; set; }
        public float MinRadius { get; set; }
        public float MaxRadius { get; set; }
    }
}