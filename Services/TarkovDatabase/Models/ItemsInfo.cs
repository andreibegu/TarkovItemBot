using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TarkovItemBot.Services
{
    public class ItemsInfo
    {
        public int Total { get; set; }
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime Modified { get; set; }
        public Dictionary<ItemKind, KindInfo> Kinds { get; set; }
    }

    public class KindInfo
    {
        public int Count { get; set; }
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime Modified { get; set; }
    }
}
