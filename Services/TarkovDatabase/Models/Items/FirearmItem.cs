using Discord;
using Humanizer;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TarkovItemBot.Services.TarkovDatabase
{
    public class FirearmItem : ModifiableItem
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

        public override EmbedBuilder ToEmbedBuilder()
        {
            var builder = base.ToEmbedBuilder();

            builder.AddField("Type", Type.Transform(To.TitleCase), true);
            builder.AddField("Class", Class.Transform(To.TitleCase), true);
            builder.AddField("Caliber", Caliber, true);
            builder.AddField("Fire Rate", $"{RateOfFire} rpm", true);
            builder.AddField("Action", Action.Transform(To.TitleCase), true);
            builder.AddField("Foldable", FoldRetractable ? "Yes" : "No", true);
            builder.AddField("Modes", Modes.Humanize(x => x.Transform(To.TitleCase)), true);
            builder.AddField("Effective Distance", $"{EffectiveDistance} m.", true);
            builder.AddField("Ergonomics", ErgonomicsFloat, true);
            builder.AddField("Recoil", $"{RecoilVertical} vert. {RecoilHorizontal} hor.", true);

            return builder;
        }
    }
}
