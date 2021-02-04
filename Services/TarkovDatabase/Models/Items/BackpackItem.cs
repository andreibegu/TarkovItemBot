using Discord;
using Humanizer;
using System.Collections.Generic;
using System.Linq;

namespace TarkovItemBot.Services
{
    public class BackpackItem : BaseItem
    {
        public List<ContainerGrid> Grids { get; set; }
        public Penalties Penalties { get; set; }

        public override EmbedBuilder ToEmbedBuilder()
        {
            var builder = base.ToEmbedBuilder();

            builder.AddField("Grids", $"`{Grids.Count}` {"grid".ToQuantity(Grids.Count, ShowQuantityAs.None)}," +
                $"`{Grids.Sum(x => x.Width * x.Height)}` slots total", true);

            if (Penalties.Speed != 0) builder.AddField("Speed Penalty", $"{Penalties.Speed}%", true);
            if (Penalties.Mouse != 0) builder.AddField("Turning Penalty", $"{Penalties.Mouse}%", true);
            if (Penalties.Deafness != Deafness.None) builder.AddField("Deafness", Penalties.Deafness.Humanize(), true);

            return builder;
        }
    }
}
