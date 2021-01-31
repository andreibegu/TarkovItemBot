using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TarkovItemBot.Services
{
    [Kind(ItemKind.Magazine)]
    public class MagazineItem : CommonItem
    {
        public int Capacity { get; set; }
        public string Caliber { get; set; }
        [JsonPropertyName("ergonomicsFP")]
        public float ErgonomicsFloat { get; set; }
        public int Ergonomics { get; set; }
        public MagazineModifier Modifier { get; set; }
        public GridModifier GridModifier { get; set; }
        public object Compatibility { get; set; }
        public object Conflicts { get; set; }
    }
}
