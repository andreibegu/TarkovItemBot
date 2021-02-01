using Discord;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace TarkovItemBot.Services
{
    [Kind(ItemKind.Magazine)]
    public class MagazineItem : CommonItem
    {
        public int Capacity { get; set; }
        public string Caliber { get; set; }
        [JsonPropertyName("ergonomicsFP")]
        public float ErgonomicsFloat { get; set; }
        public int Ergonomics { get; set; }
        public MagazineModifier Modifier { get; set; }
        public GridModifier GridModifier { get; set; }
        public object Compatibility { get; set; }
        public object Conflicts { get; set; }

        public override EmbedBuilder ToEmbedBuilder()
        {
            var builder = base.ToEmbedBuilder();

            builder.AddField("Capacity", $"{Capacity} rounds", true);
            builder.AddField("Caliber", Caliber, true);

            if (ErgonomicsFloat != 0) builder.AddField("Ergonomics", ErgonomicsFloat.ToString("+0.00;-#.00"), true);

            if (Modifier.CheckTime != 0) builder.AddField("Check Time", $"{Modifier.CheckTime:+0.00;-#.00}%", true);
            if (Modifier.LoadUnload != 0) builder.AddField("Load/Unload Time", $"{Modifier.LoadUnload:+0.00;-#.00}%", true);

            var stringBuilder = new StringBuilder();

            if (GridModifier.Height != 0) stringBuilder.AppendLine($"`{GridModifier.Height:+0;-#}` height");
            if (GridModifier.Width != 0) stringBuilder.AppendLine($"`{GridModifier.Height:+0;-#}` width");

            var grid = stringBuilder.ToString();
            if (!string.IsNullOrWhiteSpace(grid)) builder.AddField("Grid", grid, true);

            return builder;
        }
    }
}
