using Discord;
using Humanizer;
using System.Collections.Generic;

namespace TarkovItemBot.Services
{
    [Kind(ItemKind.Container)]
    public class ContainerItem : CommonItem
    {
        public List<ContainerGrid> Grids { get; set; }

        public override EmbedBuilder ToEmbedBuilder()
        {
            var builder = base.ToEmbedBuilder();

            builder.AddField("Grids", Grids.Humanize(x => $"{x.Height}x{x.Width} ({x.Height * x.Width})"), true);

            return builder;
        }
    }
}
