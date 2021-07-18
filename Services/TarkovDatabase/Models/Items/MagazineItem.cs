using Discord;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using TarkovItemBot.Helpers;

namespace TarkovItemBot.Services.TarkovDatabase
{
    public class MagazineItem : CommonItem, IAttachableItem
    {
        public int Capacity { get; set; }
        public string Caliber { get; set; }
        [JsonPropertyName("ergonomicsFP")]
        public float ErgonomicsFloat { get; set; }
        public int Ergonomics { get; set; }
        public float MalfunctionChance { get; set; }
        public MagazineModifier Modifier { get; set; }
        public GridModifier GridModifier { get; set; }
        public IReadOnlyDictionary<ItemKind, IReadOnlyList<string>> Compatibility { get; set; }
        public IReadOnlyDictionary<ItemKind, IReadOnlyList<string>> Conflicts { get; set; }

        public override EmbedBuilder ToEmbedBuilder()
        {
            var builder = base.ToEmbedBuilder();

            builder.AddField("Capacity", $"{Capacity} rounds", true);
            builder.AddField("Caliber", Caliber, true);

            if (MalfunctionChance != 0) builder.AddField("Malfunction Chance", $"{MalfunctionChance:0.00;-#.00}%", true);
            if (ErgonomicsFloat != 0) builder.AddField("Ergonomics", ErgonomicsFloat.ToString("+0.00;-#.00"), true);

            if (Modifier.CheckTime != 0) builder.AddField("Check Time", $"{Modifier.CheckTime:+0.00;-#.00}%", true);
            if (Modifier.LoadUnload != 0) builder.AddField("Load/Unload Time", $"{Modifier.LoadUnload:+0.00;-#.00}%", true);

            builder.AddGridModifier(GridModifier);

            return builder;
        }
    }
}
