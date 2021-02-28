using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using TarkovItemBot.Helpers;

namespace TarkovItemBot.Services.TarkovDatabase
{
    public class ItemIndex
    {
        public int Total { get; set; }
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime Modified { get; set; }
        public IReadOnlyDictionary<ItemKind, KindInfo> Kinds { get; set; }
    }

    public class KindInfo
    {
        public int Count { get; set; }
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime Modified { get; set; }
    }
}
