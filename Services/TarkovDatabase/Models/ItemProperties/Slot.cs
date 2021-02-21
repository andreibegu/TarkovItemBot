using System.Collections.Generic;

namespace TarkovItemBot.Services
{
    public record Slot(IReadOnlyDictionary<ItemKind, IReadOnlyList<string>> Filter, bool Required);
}
