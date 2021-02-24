using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Web;
using TarkovItemBot.Helpers;

namespace TarkovItemBot.Services.TarkovDatabase
{
    public class Location
    {
        [JsonPropertyName("_id")]
        public string Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public int MinPlayers { get; set; }
        public int MaxPlayers { get; set; }
        public int EscapeTime { get; set; }
        public bool Insurance { get; set; }
        public bool Available { get; set; }
        public IReadOnlyCollection<Exit> Exits { get; set; }
        public object Bosses { get; set; }
        [JsonPropertyName("_modified")]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime Modified { get; set; }
        public string WikiUrl => $"https://escapefromtarkov.gamepedia.com/{HttpUtility.UrlEncode(Name.Replace(" ", "_"))}";
    }
}
