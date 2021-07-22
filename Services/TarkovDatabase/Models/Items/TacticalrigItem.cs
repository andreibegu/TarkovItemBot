using Disqord;
using System.Collections.Generic;
using TarkovItemBot.Helpers;

namespace TarkovItemBot.Services.TarkovDatabase
{
    class TacticalrigItem : CommonItem
    {
        public IReadOnlyCollection<ContainerGrid> Grids { get; set; }
        public Penalties Penalties { get; set; }
        public ArmorProperties Armor { get; set; }

        public override LocalEmbed ToEmbed()
        {
            var embed = base.ToEmbed();

            if (Armor != null)
            {
                embed.AddArmorProperties(Armor);
            }

            if (Penalties.Speed != 0) embed.AddField("Speed Penalty", $"{Penalties.Speed}%", true);
            if (Penalties.Mouse != 0) embed.AddField("Turning Penalty", $"{Penalties.Mouse}%", true);

            embed.AddGrids(Grids);

            return embed;
        }
    }
}
