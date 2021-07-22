using Disqord;
using Humanizer;
using System.Text.Json.Serialization;

namespace TarkovItemBot.Services.TarkovDatabase
{
    public class GrenadeItem : CommonItem
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

        public override LocalEmbed ToEmbed()
        {
            var embed = base.ToEmbed();

            embed.AddField("Type", Type.Transform(To.TitleCase), true);
            embed.AddField("Delay", $"{Delay} sec.", true);

            if (MinDistance != 0) embed.AddField("Distance", $"{MinDistance}-{MaxDistance} m.", true);
            if (FragmentCount != 0) embed.AddField("Fragments", FragmentCount, true);
            if (EmitTime != 0) embed.AddField("Burn time", $"{EmitTime} sec.", true);
            if (ContusionDistance != 0) embed.AddField("Contusion distance", $"{ContusionDistance} m.", true);

            return embed;
        }
    }
}
