namespace TarkovItemBot.Services
{
    [Kind(ItemKind.Medical)]
    public class MedicalItem : CommonItem
    {
        public string Type { get; set; }
        public int Resources { get; set; }
        public int ResourceRate { get; set; }
        public float UseTime { get; set; }
        public Effects Effects { get; set; }
    }
}
