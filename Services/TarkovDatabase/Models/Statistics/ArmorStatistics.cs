using System;
using System.Text.Json.Serialization;
using TarkovItemBot.Helpers;

namespace TarkovItemBot.Services.TarkovDatabase
{
    public class ArmorStatistics
    {
        [JsonPropertyName("_id")]
        public string Id { get; set; }
        public string Ammo { get; set; }
        public ItemReference Armor { get; set; }
        public int Distance { get; set; }
        public float[] PenetrationChance { get; set; }
        [JsonPropertyName("avgShotsToDestruct")]
        public Statistics AverageShotsToDestruction { get; set; }
        [JsonPropertyName("avgShotsTo50Damage")]
        public Statistics AverageShotsTo50Damage { get; set; }
        [JsonPropertyName("_modified")]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime Modified { get; set; }
    }
}
