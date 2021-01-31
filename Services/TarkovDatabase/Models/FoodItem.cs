namespace TarkovItemBot.Services
{
    [Kind(ItemKind.Food)]
    public class FoodItem : CommonItem
    {
        public string Type { get; set; }
        public int Resources { get; set; }
        public float UseTime { get; set; }
        public Effects Effects { get; set; }
    }
}
