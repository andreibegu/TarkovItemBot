using Disqord;
using Humanizer;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using TarkovItemBot.Helpers;

namespace TarkovItemBot.Services.TarkovDatabase
{
    public class ModificationItem : CommonItem, IModifiableItem, IAttachableItem
    {
        [JsonPropertyName("ergonomicsFP")]
        public float ErgonomicsFloat { get; set; }
        public int Ergonomics { get; set; }
        public float Accuracy { get; set; }
        public float Recoil { get; set; }
        public int RaidModdable { get; set; }
        public GridModifier GridModifier { get; set; }
        public IReadOnlyDictionary<ItemKind, IReadOnlyList<string>> Compatibility { get; set; }
        public IReadOnlyDictionary<ItemKind, IReadOnlyList<string>> Conflicts { get; set; }
        public IReadOnlyDictionary<string, Slot> Slots { get; set; }

        public override LocalEmbed ToEmbed()
        {
            var embed = base.ToEmbed();

            if (ErgonomicsFloat != 0) embed.AddField("Ergonomics", ErgonomicsFloat.ToString("+0.00;-#.00"), true);
            if (Accuracy != 0) embed.AddField("Accuracy", Accuracy.ToString("+0.00;-#.00"), true);
            if (Recoil != 0) embed.AddField("Recoil", Recoil.ToString("+0.00;-#.00"), true);

            embed.AddGridModifier(GridModifier);

            return embed;
        }
    }

    public class BarrelItem : ModificationItem
    {
        public float Length { get; set; }
        public float Velocity { get; set; }
        public bool Supressor { get; set; }
        public float DurabilityBurn { get; set; }
        public float HeatFactor { get; set; }
        public float CoolFactor { get; set; }
        public float CenterOfImpact { get; set; }

        public override LocalEmbed ToEmbed()
        {
            var embed = base.ToEmbed();

            embed.AddField("Length", $"{Length} mm", true);
            embed.AddField("Supresses", Supressor ? "Yes" : "No", true);

            var accuracy = 100 * CenterOfImpact / 2.9089;

            if (accuracy != 0) embed.AddField("Accuracy", $"{accuracy:0.00} MOA", true);
            if (Velocity != 0) embed.AddField("Velocity", Velocity.ToString("+0.00;-#.00"), true);

            if (1 - DurabilityBurn != 0) embed.AddField("Durability Burn", $"{(1 - DurabilityBurn) * 100:+0.00;-#.00}%", true);
            if (HeatFactor != 0) embed.AddField("Heat", $"{(1 - HeatFactor) * 100:+0.00;-#.00}%", true);
            if (CoolFactor != 0) embed.AddField("Cooling", $"{(1 - CoolFactor) * 100:+0.00;-#.00}%", true);

            return embed;
        }
    }

    public class BipodItem : ModificationItem
    {
    }

    public class ChargeItem : ModificationItem
    {
    }

    public class DeviceItem : ModificationItem
    {
        public string Type { get; set; }
        public IReadOnlyCollection<string> Modes { get; set; }

        public override LocalEmbed ToEmbed()
        {
            var embed = base.ToEmbed();

            embed.AddField("Type", Type.Transform(To.TitleCase), true);
            embed.AddField("Modes", Modes.Humanize(x => x.Transform(To.TitleCase)), true);

            return embed;
        }
    }

    public class ForegripItem : ModificationItem
    {
    }

    public class GasblockItem : ModificationItem
    {
        public float DurabilityBurn { get; set; }
        public float HeatFactor { get; set; }
        public float CoolFactor { get; set; }

        public override LocalEmbed ToEmbed()
        {
            var embed = base.ToEmbed();

            if (1 - DurabilityBurn != 0) embed.AddField("Durability Burn", $"{(1 - DurabilityBurn) * 100:+0.00;-#.00}%", true);
            if (HeatFactor != 0) embed.AddField("Heat", $"{(1 - HeatFactor) * 100:+0.00;-#.00}%", true);
            if (CoolFactor != 0) embed.AddField("Cooling", $"{(1 - CoolFactor) * 100:+0.00;-#.00}%", true);

            return embed;
        }
    }

    public class HandguardItem : ModificationItem
    {
        public float HeatFactor { get; set; }
    }

    public class LauncherItem : ModificationItem
    {
        public string Caliber { get; set; }

        //public override LocalEmbed ToEmbed()
        //    => base.ToEmbed().AddField("Caliber", Caliber, true);
    }

    public class MountItem : ModificationItem
    {
        public float HeatFactor { get; set; }
    }

    public class MuzzleItem : ModificationItem
    {
        public string Type { get; set; }
        public float Velocity { get; set; }
        public float Loudness { get; set; }
        public float DurabilityBurn { get; set; }
        public float HeatFactor { get; set; }
        public float CoolFactor { get; set; }

        public override LocalEmbed ToEmbed()
        {
            var embed = base.ToEmbed();

            embed.AddField("Type", Type.Transform(To.TitleCase), true);
            embed.AddField("Loudness", Loudness.ToString("+0.00;-#.00"), true);

            if (Velocity != 0) embed.AddField("Velocity", Velocity.ToString("+0.00;-#.00"), true);

            if (1 - DurabilityBurn != 0) embed.AddField("Durability Burn", $"{(1 - DurabilityBurn) * 100:+0.00;-#.00}%", true);
            if (HeatFactor != 0) embed.AddField("Heat", $"{(1 - HeatFactor) * 100:+0.00;-#.00}%", true);
            if (CoolFactor != 0) embed.AddField("Cooling", $"{(1 - CoolFactor) * 100:+0.00;-#.00}%", true);

            return embed;
        }
    }

    public class PistolgripItem : ModificationItem
    {
    }

    public class ReceiverItem : ModificationItem
    {
        public float Velocity { get; set; }
        public float DurabilityBurn { get; set; }
        public float HeatFactor { get; set; }
        public float CoolFactor { get; set; }

        public override LocalEmbed ToEmbed()
        {
            var embed = base.ToEmbed();

            if (Velocity != 0) embed.AddField("Velocity", Velocity.ToString("+0.00;-#.00"), true);

            if (1 - DurabilityBurn != 0) embed.AddField("Durability Burn", $"{(1 - DurabilityBurn) * 100:+0.00;-#.00}%", true);
            if (HeatFactor != 0) embed.AddField("Heat", $"{(1 - HeatFactor) * 100:+0.00;-#.00}%", true);
            if (CoolFactor != 0) embed.AddField("Cooling", $"{(1 - CoolFactor) * 100:+0.00;-#.00}%", true);

            return embed;
        }
    }

    public class SightItem : ModificationItem
    {
        public string Type { get; set; }
        public IReadOnlyCollection<string> Magnification { get; set; }
        public bool VariableZoom { get; set; }
        public IReadOnlyCollection<int> ZeroDistances { get; set; }

        public override LocalEmbed ToEmbed()
        {
            var embed = base.ToEmbed();

            embed.AddField("Type", Type.Transform(To.TitleCase), true);
            embed.AddField("Variable Zoom", VariableZoom ? "Yes" : "No", true);
            embed.AddField("Magnification", Magnification.Humanize(), true);
            embed.AddField("Zero Distances", ZeroDistances.Humanize(x => $"{x} m."), true);

            return embed;
        }
    }

    public class SightSpecialItem : SightItem
    {
        public IReadOnlyCollection<string> Modes { get; set; }
        public Color Color { get; set; }
        public string Noise { get; set; }
    }

    public class StockItem : ModificationItem
    {
        public bool FoldRetractable { get; set; }
        public float HeatFactor { get; set; }
        public float CoolFactor { get; set; }

        public override LocalEmbed ToEmbed()
        {
            var embed = base.ToEmbed();

            if (HeatFactor != 0) embed.AddField("Heat", $"{(1 - HeatFactor) * 100:+0.00;-#.00}%", true);
            if (CoolFactor != 0) embed.AddField("Cooling", $"{(1 - CoolFactor) * 100:+0.00;-#.00}%", true);

            embed.AddField("Foldable", FoldRetractable ? "Yes" : "No", true);

            return embed;
        }
    }

    public class GogglesItem : ModificationItem
    {
        public string Type { get; set; }
        public IReadOnlyCollection<string> Modes { get; set; }
        public Color Color { get; set; }
        public string Noise { get; set; }

        public override LocalEmbed ToEmbed()
            => base.ToEmbed().AddField("Type", Type.Transform(To.TitleCase), true);
    }

    public class AuxiliaryItem : ModificationItem
    {
        public float DurabilityBurn { get; set; }
        public float HeatFactor { get; set; }
        public float CoolFactor { get; set; }

        public override LocalEmbed ToEmbed()
        {
            var embed = base.ToEmbed();

            if (1 - DurabilityBurn != 0) embed.AddField("Durability Burn", $"{(1 - DurabilityBurn) * 100:+0.00;-#.00}%", true);
            if (HeatFactor != 0) embed.AddField("Heat", $"{(1 - HeatFactor) * 100:+0.00;-#.00}%", true);
            if (CoolFactor != 0) embed.AddField("Cooling", $"{(1 - CoolFactor) * 100:+0.00;-#.00}%", true);

            return embed;
        }
    }
}
