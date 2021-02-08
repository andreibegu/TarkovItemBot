using Discord;
using Humanizer;
using System.Collections.Generic;
using System.Linq;

namespace TarkovItemBot.Services
{
    public class ContainerItem : BaseItem
    {
        public List<ContainerGrid> Grids { get; set; }

        public override EmbedBuilder ToEmbedBuilder()
        {
            var builder = base.ToEmbedBuilder();

            builder.AddField("Grids", $"`{Grids.Count}` {"grid".ToQuantity(Grids.Count, ShowQuantityAs.None)}, " +
                $"`{Grids.Sum(x => x.Width * x.Height)}` slots total", true);

            return builder;
        }
    }
}
