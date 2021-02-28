using System.Collections.Generic;

namespace TarkovItemBot.Services.TarkovDatabase
{
    public class ModifiableItem : CommonItem
    {
        public IReadOnlyDictionary<string, Slot> Slots { get; set; }
    }
}
