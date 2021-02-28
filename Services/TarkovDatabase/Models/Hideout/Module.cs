using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using TarkovItemBot.Helpers;

namespace TarkovItemBot.Services.TarkovDatabase
{
    public class Module
    {
        [JsonPropertyName("_id")]
        public string Id { get; set; }
        public string Name { get; set; }
        public bool RequiresPower { get; set; }
        public IReadOnlyList<Stage> Stages { get; set; }
        [JsonPropertyName("_modified")]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime Modified { get; set; }
    }
}
