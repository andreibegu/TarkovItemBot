namespace TarkovItemBot.Services
{
    [Kind(ItemKind.Ammunition)]
    public class AmmunitionItem : CommonItem
    {
        public string Caliber { get; set; }
        public string Type { get; set; }
        public bool Tracer { get; set; }
        public string TracerColor { get; set; }
        public bool Subsonic { get; set; }
        public float Velocity { get; set; }
        public float BallisticCoef { get; set; }
        public float Damage { get; set; }
        public float Penetration { get; set; }
        public float ArmorDamage { get; set; }
        public FragmentationProperties Fragmentation { get; set; }
        public AmmunitionEffectProperties Effects { get; set; }
        public int Pellets { get; set; }
        public int Projectiles { get; set; }
        public WeaponModifiers WeaponModifier { get; set; }
    }
}
