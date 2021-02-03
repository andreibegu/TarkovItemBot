using Discord;
using Humanizer;
using System.Collections.Generic;
using System.Text;

namespace TarkovItemBot.Services
{
    public class BackpackItem : CommonItem
    {
        public List<ContainerGrid> Grids { get; set; }
        public Penalties Penalties { get; set; }

        public override EmbedBuilder ToEmbedBuilder()
        {
            var builder = base.ToEmbedBuilder();

            builder.AddField("Grids", Grids.Humanize(x => $"{x.Height}x{x.Width} ({x.Height * x.Width})"), true);

            if (Penalties.Speed != 0) builder.AddField("Speed Penalty", $"{Penalties.Speed}%", true);
            if (Penalties.Mouse != 0) builder.AddField("Turning Penalty", $"{Penalties.Mouse}%", true);
            if (Penalties.Deafness != Deafness.None) builder.AddField("Deafness", Penalties.Deafness.Humanize(), true);

            return builder;
        }
    }
}
