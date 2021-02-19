using Discord;
using Humanizer;
using System.Collections.Generic;
using TarkovItemBot.Helpers;

namespace TarkovItemBot.Services
{
    public class BackpackItem : BaseItem
    {
        public IReadOnlyCollection<ContainerGrid> Grids { get; set; }
        public Penalties Penalties { get; set; }

        public override EmbedBuilder ToEmbedBuilder()
        {
            var builder = base.ToEmbedBuilder();

            if (Penalties.Speed != 0) builder.AddField("Speed Penalty", $"{Penalties.Speed}%", true);
            if (Penalties.Mouse != 0) builder.AddField("Turning Penalty", $"{Penalties.Mouse}%", true);
            if (Penalties.Deafness != Deafness.None) builder.AddField("Deafness", Penalties.Deafness.Humanize(), true);

            builder.AddGrids(Grids);

            return builder;
        }
    }
}
