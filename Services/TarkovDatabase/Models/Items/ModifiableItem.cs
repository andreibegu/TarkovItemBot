using System.Collections.Generic;

namespace TarkovItemBot.Services
{
    public class ModifiableItem : BaseItem
    {
        public IReadOnlyDictionary<string, Slot> Slots { get; set; }
    }
}
