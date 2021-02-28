using System.Collections.Generic;

namespace TarkovItemBot.Services.TarkovDatabase
{
    public class ContainerGrid
    {
        public string Id { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public float MaxWeight { get; set; }
        public IReadOnlyDictionary<ItemKind, IReadOnlyList<string>> Filter { get; set; }
    }
}
