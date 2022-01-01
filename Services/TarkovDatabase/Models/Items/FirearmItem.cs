using Disqord;
using Humanizer;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TarkovItemBot.Services.TarkovDatabase
{
    public class FirearmItem : CommonItem, IModifiableItem
    {
        public string Type { get; set; }
        public string Class { get; set; }
        public string Caliber { get; set; }
        [JsonPropertyName("rof")]
        public int RateOfFire { get; set; }
        public int BurstRounds { get; set; }
        public string Action { get; set; }
        public IReadOnlyCollection<string> Modes { get; set; }
        public float Velocity { get; set; }
        [JsonPropertyName("effectiveDist")]
        public int EffectiveDistance { get; set; }
        [JsonPropertyName("ergonomicsFP")]
        public float ErgonomicsFloat { get; set; }
        public int Ergonomics { get; set; }
        public bool FoldRetractable { get; set; }
        public int RecoilVertical { get; set; }
        public int RecoilHorizontal { get; set; }
        public float OperatingResources { get; set; }
        public float MalfunctionChance { get; set; }
        public float DurabilityRatio { get; set; }
        public float HeatFactor { get; set; }
        public float CoolFactor { get; set; }
        public float CenterOfImpact { get; set; }
        public IReadOnlyDictionary<string, Slot> Slots { get; set; }

        public override LocalEmbed ToEmbed()
        {
            var embed = base.ToEmbed();

            embed.AddField("Type", Type.Transform(To.TitleCase), true);
            embed.AddField("Class", Class.Transform(To.TitleCase), true);
            embed.AddField("Caliber", Caliber, true);
            embed.AddField("Fire Rate", $"{RateOfFire} rpm", true);
            embed.AddField("Action", Action.Transform(To.TitleCase), true);
            embed.AddField("Foldable", FoldRetractable ? "Yes" : "No", true);
            embed.AddField("Modes", Modes.Humanize(x => x.Transform(To.TitleCase)), true);
            embed.AddField("Effective Distance", $"{EffectiveDistance} m.", true);
            embed.AddField("Ergonomics", ErgonomicsFloat, true);
            embed.AddField("Recoil", $"{RecoilVertical} vert. {RecoilHorizontal} hor.", true);
            embed.AddField("Malfunction Chance", $"{MalfunctionChance:0.00;-#.00}%", true);

            var accuracy = 100 * CenterOfImpact / 2.9089;
            if (accuracy != 0) embed.AddField("Accuracy", $"{accuracy:0.00} MOA", true);

            return embed;
        }
    }
}
