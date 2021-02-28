using System.Collections.Generic;

namespace TarkovItemBot.Services.TarkovDatabase
{
    public record Slot(IReadOnlyDictionary<ItemKind, IReadOnlyList<string>> Filter, bool Required);
}
