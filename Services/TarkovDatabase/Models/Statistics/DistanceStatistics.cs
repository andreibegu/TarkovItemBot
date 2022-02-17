using System;
using System.Text.Json.Serialization;
using TarkovItemBot.Helpers;

namespace TarkovItemBot.Services.TarkovDatabase
{
    public class DistanceStatistics
    {
        [JsonPropertyName("_id")]
        public string Id { get; set; }
        public string Reference { get; set; }
        public uint Distance { get; set; }
        public float Velocity { get; set; }
        public float Damage { get; set; }
        public float PenetrationPower { get; set; }
        public float TimeOfFlight { get; set; }
        public float Drop { get; set; }
        [JsonPropertyName("_modified")]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime Modified { get; set; }
    }
}
