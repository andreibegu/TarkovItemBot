using System.Collections.Generic;

namespace TarkovItemBot.Services
{
    [Kind(ItemKind.Clothing)]
    public class ClothingItem : CommonItem
    {
        public List<string> Blocking { get; set; }
        public Penalties Penalties { get; set; }
        public Dictionary<string, Slot> Slots { get; set; }
        public string Type { get; set; }
    }
}
