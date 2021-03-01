
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using TarkovItemBot.Helpers;

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
        [JsonConverter(typeof(SecondsTimeSpanConverter))]
        public TimeSpan ConstructionTime { get; set; }

    }
}
