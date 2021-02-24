using Discord;
using System.Text.Json.Serialization;

namespace TarkovItemBot.Services.TarkovDatabase
{
    public class HeadphoneItem : BaseItem
    {
        [JsonPropertyName("ambientVol")]
        public float AmbientVolume { get; set; }
        [JsonPropertyName("dryVol")]
        public float DryVolume { get; set; }
        public float Distortion { get; set; }
        [JsonPropertyName("hpf")]
        public HighPass HighPass { get; set; }
        public Compressor Compressor { get; set; }

        public override EmbedBuilder ToEmbedBuilder()
        {
            var builder = base.ToEmbedBuilder();

            builder.AddField("Ambient Volume", $"{AmbientVolume:+0.00;-#.00} dB", true);
            builder.AddField("Distortion", $"{Distortion * 100}%", true);
            builder.AddField("Cutoff Frequency", $"< {HighPass.CutoffFrequency} Hz", true);

            return builder;
        }
    }
}
