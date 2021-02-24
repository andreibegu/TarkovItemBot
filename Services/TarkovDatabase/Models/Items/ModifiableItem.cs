using System.Collections.Generic;

namespace TarkovItemBot.Services.TarkovDatabase
{
    public class ModifiableItem : BaseItem
    {
        public IReadOnlyDictionary<string, Slot> Slots { get; set; }
    }
}
