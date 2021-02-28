namespace TarkovItemBot.Services.TarkovDatabase
{
    public class ItemReference
    {
        public string Id { get; set; }
        public int Count { get; set; }
        public int Resources { get; set; }
        public ItemKind Kind { get; set; }
    }
}
