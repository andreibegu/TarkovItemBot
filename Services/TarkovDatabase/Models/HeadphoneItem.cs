using System.Text.Json.Serialization;

namespace TarkovItemBot.Services
{
    [Kind(ItemKind.Headphone)]
    public class HeadphoneItem : CommonItem
    {
        [JsonPropertyName("ambientVol")]
        public float AmbientVolume { get; set; }
        [JsonPropertyName("dryVol")]
        public float DryVolume { get; set; }
        public float Distortion { get; set; }
        [JsonPropertyName("hpf")]
        public HighPass HighPass { get; set; }
        public Compressor Compressor { get; set; }

    }
}
