using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TarkovItemBot.Services
{
    [Kind(ItemKind.Firearm)]
    public class FirearmItem : CommonItem
    {
        public string Type { get; set; }
        public string Class { get; set; }
        public string Caliber { get; set; }
        [JsonPropertyName("rof")]
        public int RateOfFire { get; set; }
        public int BurstRounds { get; set; }
        public string Action { get; set; }
        public List<string> Modes { get; set; }
        public float Velocity { get; set; }
        [JsonPropertyName("effectiveDist")]
        public int EffectiveDistance { get; set; }
        [JsonPropertyName("ergonomicsFP")]
        public float ErgonomicsFloat { get; set; }
        public int Ergonomics { get; set; }
        public bool FoldRetractable { get; set; }
        public int RecoilVertical { get; set; }
        public int RecoilHorizontal { get; set; }
        public Dictionary<string, Slot> Slots { get; set; }
    }
}
