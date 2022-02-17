using Disqord;
using Humanizer;
using System.Text.Json.Serialization;
using TarkovItemBot.Helpers;

namespace TarkovItemBot.Services.TarkovDatabase
{
    public class AmmunitionItem : CommonItem
    {
        public string Caliber { get; set; }
        public string Type { get; set; }
        public bool Tracer { get; set; }
        public string TracerColor { get; set; }
        public bool Subsonic { get; set; }
        public float Velocity { get; set; }
        public float BallisticCoeficient { get; set; }
        public float Damage { get; set; }
        public float Penetration { get; set; }
        public float ArmorDamage { get; set; }
        public float Retardation { get; set; }
        public FragmentationProperties Fragmentation { get; set; }
        public AmmunitionEffectProperties Effects { get; set; }
        public int Pellets { get; set; }
        public int Projectiles { get; set; }
        public float MisfireChance { get; set; }
        public float FailureToFeedChance { get; set; }
        public WeaponModifiers WeaponModifier { get; set; }
        [JsonPropertyName("grenadeProps")]
        public GrenadeProperties GrenadeProperties { get; set; }

        public string BallisticsLink 
            => $"https://tarkov-ballistics.com/charts/{Caliber.Slugify()}/{Name.Slugify()}.html";

        public override LocalEmbed ToEmbed()
        {
            var builder = base.ToEmbed();

            builder.AddField("Caliber", Caliber, true);
            builder.AddField("Type", Type.Transform(To.TitleCase), true);
            builder.AddField("Tracer", Tracer ? TracerColor.Replace("tracer", "") : "No", true);
            builder.AddField("Subsonic", Subsonic ? "Yes" : "No", true);
            builder.AddField("Damage", $"{Damage} ({ArmorDamage} to armor)", true);
            builder.AddField("Penetration", Penetration, true);
            builder.AddField("Velocity", $"{Velocity} m/s", true);

            builder.AddField("Fragmentation", $"{Fragmentation.Chance * 100}% ({Fragmentation.Min}-{Fragmentation.Max})", true);

            if (Projectiles != 1) builder.AddField("Projectiles", Projectiles, true);
            if (MisfireChance != 0) builder.AddField("Misfire Chance", $"{MisfireChance * 100} %", true);
            if (FailureToFeedChance != 0) builder.AddField("FTF Chance", $"{FailureToFeedChance * 100} %", true);

            if (WeaponModifier.Accuracy != 0) builder.AddField("Accuracy", $"{WeaponModifier.Accuracy:+0.00;-#.00}%", true);
            if (WeaponModifier.Recoil != 0) builder.AddField("Recoil", $"{WeaponModifier.Recoil:+0.00;-#.00}%", true);
            if (WeaponModifier.DurabilityBurn != 0) builder.AddField("Durability Burn", $"{(WeaponModifier.DurabilityBurn - 1) * 100:+0.00;-#.00}%", true);
            if (WeaponModifier.HeatFactor != 0) builder.AddField("Heat", $"{(WeaponModifier.HeatFactor - 1) * 100:+0.00;-#.00}%", true);

            if (Effects.HeavyBleedingChance != 0) builder.AddField("Heavy Bleeding Chance", $"{Effects.HeavyBleedingChance * 100}%", true);
            if (Effects.LightBleedingChance != 0) builder.AddField("Light Bleeding Chance", $"{Effects.LightBleedingChance * 100}%", true);

            if (Type == "grenade")
            {
                builder.AddField("Delay", $"{GrenadeProperties.Delay} sec.", true);
                builder.AddField("Radius", $"{GrenadeProperties.MinRadius}-{GrenadeProperties.MaxRadius}", true);
                builder.AddField("Fragments", GrenadeProperties.FragmentCount, true);
            }

            return builder;
        }
    }
}
