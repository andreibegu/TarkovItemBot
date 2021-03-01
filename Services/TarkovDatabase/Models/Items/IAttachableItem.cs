using System.Collections.Generic;

namespace TarkovItemBot.Services.TarkovDatabase
{
    public interface IAttachableItem
    {
        public IReadOnlyDictionary<ItemKind, IReadOnlyList<string>> Compatibility { get; set; }
    }
}
