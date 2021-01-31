using System.Text.Json.Serialization;

namespace TarkovItemBot.Services
{
    [Kind(ItemKind.Grenade)]
    public class GrenadeItem : CommonItem
    {
        public string Type { get; set; }
        public float Delay { get; set; }
        [JsonPropertyName("fragCount")]
        public float FragmentCount { get; set; }
        public float MinDistance { get; set; }
        public float MaxDistance { get; set; }
        public float ContusionDistance { get; set; }
        public float Strength { get; set; }
        public float EmitTime { get; set; }
    }
}
