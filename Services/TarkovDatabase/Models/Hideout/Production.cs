using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using TarkovItemBot.Helpers;

namespace TarkovItemBot.Services.TarkovDatabase
{
    public class Production
    {
        [JsonPropertyName("_id")]
        public string Id { get; set; }
        public string Module { get; set; }
        [JsonPropertyName("requiredMods")]
        public IReadOnlyList<ModuleReference> RequiredModules { get; set; }
        public IReadOnlyList<ItemReference> Materials { get; set; }
        public IReadOnlyList<ItemReference> Outcome { get; set; }
        public int Duration { get; set; }
        [JsonPropertyName("_modified")]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime Modified { get; set; }
    }
}
