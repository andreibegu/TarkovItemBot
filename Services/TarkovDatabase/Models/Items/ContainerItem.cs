using Disqord;
using System.Collections.Generic;
using TarkovItemBot.Helpers;

namespace TarkovItemBot.Services.TarkovDatabase
{
    public class ContainerItem : CommonItem
    {
        public IReadOnlyCollection<ContainerGrid> Grids { get; set; }

        public override LocalEmbed ToEmbed()
        {
            var embed = base.ToEmbed();

            embed.AddGrids(Grids);

            return embed;
        }
    }
}
