using System.Text.Json.Serialization;

namespace TarkovItemBot.Services.TarkovDatabase
{
    public class HighPass
    {
        [JsonPropertyName("cutoffFreq")]
        public float CutoffFrequency { get; set; }
        public float Resonance { get; set; }
    }
}
