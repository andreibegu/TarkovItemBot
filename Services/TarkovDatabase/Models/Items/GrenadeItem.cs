using Discord;
using Humanizer;
using System.Text.Json.Serialization;

namespace TarkovItemBot.Services
{
    public class GrenadeItem : BaseItem
    {
        public string Type { get; set; }
        public float Delay { get; set; }
        [JsonPropertyName("fragCount")]
        public float FragmentCount { get; set; }
        public float MinDistance { get; set; }
        public float MaxDistance { get; set; }
        public float ContusionDistance { get; set; }
        public float Strength { get; set; }
        public float EmitTime { get; set; }

        public override EmbedBuilder ToEmbedBuilder()
        {
            var builder = base.ToEmbedBuilder();

            builder.AddField("Type", Type.Transform(To.TitleCase), true);
            builder.AddField("Delay", $"{Delay} sec.", true);

            if (MinDistance != 0) builder.AddField("Distance", $"{MinDistance}-{MaxDistance} m.", true);
            if (FragmentCount != 0) builder.AddField("Fragments", FragmentCount, true);
            if (EmitTime != 0) builder.AddField("Burn time", $"{EmitTime} sec.", true);
            if (ContusionDistance != 0) builder.AddField("Contusion distance", $"{ContusionDistance} m.", true);

            return builder;
        }
    }
}
