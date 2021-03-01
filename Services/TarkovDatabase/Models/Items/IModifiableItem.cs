using System.Collections.Generic;

namespace TarkovItemBot.Services.TarkovDatabase
{
    public interface IModifiableItem
    {
        public IReadOnlyDictionary<string, Slot> Slots { get; set; }
    }
}
