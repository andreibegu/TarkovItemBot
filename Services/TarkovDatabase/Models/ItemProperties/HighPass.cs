using System.Text.Json.Serialization;

namespace TarkovItemBot.Services
{
    public class HighPass
    {
        [JsonPropertyName("cutoffFreq")]
        public float CutoffFrequency { get; set; }
        public float Resonance { get; set; }
    }
}
