using System.Collections.Generic;

namespace TarkovItemBot.Services
{
    [Kind(ItemKind.Backpack)]
    public class BackpackItem : CommonItem
    {
        public List<ContainerGrid> Grids { get; set; }
        public Penalties Penalties { get; set; }
    }
}
