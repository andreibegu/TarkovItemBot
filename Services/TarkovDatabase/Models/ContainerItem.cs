using System.Collections.Generic;

namespace TarkovItemBot.Services
{
    [Kind(ItemKind.Container)]
    public class ContainerItem : CommonItem
    {
        public List<ContainerGrid> Grids { get; set; }
    }
}
