using System.Collections.Generic;

namespace TarkovItemBot.Services
{
    [Kind(ItemKind.Armor)]
    public class ArmorItem : CommonItem
    {
        public ArmorType Type { get; set; }
        public ArmorProperties Armor { get; set; }
        public Penalties Penalties { get; set; }
        public List<BlockingZone> Blocking { get; set; }
        public Dictionary<string, Slot> Slots { get; set; }
        public object Compatibility { get; set; }
    }
}
