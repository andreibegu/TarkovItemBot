using Discord;
using System.Collections.Generic;
using TarkovItemBot.Helpers;

namespace TarkovItemBot.Services
{
    class TacticalrigItem : BaseItem
    {
        public List<ContainerGrid> Grids { get; set; }
        public Penalties Penalties { get; set; }
        public ArmorProperties Armor { get; set; }

        public override EmbedBuilder ToEmbedBuilder()
        {
            var builder = base.ToEmbedBuilder();

            if (Armor != null)
            {
                builder.AddArmorProperties(Armor);
            }

            if (Penalties.Speed != 0) builder.AddField("Speed Penalty", $"{Penalties.Speed}%", true);
            if (Penalties.Mouse != 0) builder.AddField("Turning Penalty", $"{Penalties.Mouse}%", true);

            builder.AddGrids(Grids);

            return builder;
        }
    }
}
