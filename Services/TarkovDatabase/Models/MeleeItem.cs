namespace TarkovItemBot.Services
{
    [Kind(ItemKind.Melee)]
    public class MeleeItem : CommonItem
    {
        public MeleeAttack Slash { get; set; }
        public MeleeAttack Stab { get; set; }
    }
}
