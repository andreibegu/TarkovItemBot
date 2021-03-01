
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TarkovItemBot.Services.TarkovDatabase
{
    public class Stage
    {
        public string Description { get; set; }
        public IReadOnlyList<Bonus> Bonuses { get; set; }
        public IReadOnlyList<Requirement> Requirements { get; set; }
        [JsonPropertyName("requiredMods")]
        public IReadOnlyList<ModuleReference> RequiredModules { get; set; }
        public IReadOnlyList<ItemReference> Materials { get; set; }
        public int ConstructionTime { get; set; }

    }
}
