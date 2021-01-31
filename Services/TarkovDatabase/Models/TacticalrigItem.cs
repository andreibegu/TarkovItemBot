namespace TarkovItemBot.Services
{
    [Kind(ItemKind.Tacticalrig)]
    class TacticalrigItem : CommonItem
    {
        public ContainerGrid Grids { get; set; }
        public Penalties Penalties { get; set; }
        public ArmorProperties Armor { get; set; }
    }
}
