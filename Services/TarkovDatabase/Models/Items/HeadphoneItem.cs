using Disqord;
using System.Text.Json.Serialization;

namespace TarkovItemBot.Services.TarkovDatabase
{
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

        public override LocalEmbed ToEmbed()
        {
            var embed = base.ToEmbed();

            embed.AddField("Ambient Volume", $"{AmbientVolume:+0.00;-#.00} dB", true);
            embed.AddField("Distortion", $"{Distortion * 100}%", true);
            embed.AddField("Cutoff Frequency", $"< {HighPass.CutoffFrequency} Hz", true);

            return embed;
        }
    }
}
