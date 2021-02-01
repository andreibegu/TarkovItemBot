using Discord;
using Humanizer;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace TarkovItemBot.Services
{
    [Kind(ItemKind.Modification)]
    public class ModificationItem : CommonItem
    {
        [JsonPropertyName("ergonomicsFP")]
        public float ErgonomicsFloat { get; set; }
        public int Ergonomics { get; set; }
        public float Accuracy { get; set; }
        public float Recoil { get; set; }
        public int RaidModdable { get; set; }
        public GridModifier GridModifier { get; set; }
        public Dictionary<string, Slot> Slots { get; set; }
        public object Compatibility { get; set; }
        public object Conflicts { get; set; }

        public override EmbedBuilder ToEmbedBuilder()
        {
            var builder = base.ToEmbedBuilder();

            if (ErgonomicsFloat != 0) builder.AddField("Ergonomics", ErgonomicsFloat.ToString("+0.00;-#.00"), true);
            if (Accuracy != 0) builder.AddField("Accuracy", Accuracy.ToString("+0.00;-#.00"), true);
            if (Recoil != 0) builder.AddField("Recoil", Recoil.ToString("+0.00;-#.00"), true);

            var stringBuilder = new StringBuilder();

            if (GridModifier.Height != 0) stringBuilder.AppendLine($"`{GridModifier.Height:+0;-#}` height");
            if (GridModifier.Width != 0) stringBuilder.AppendLine($"`{GridModifier.Height:+0;-#}` width");

            var grid = stringBuilder.ToString();
            if (!string.IsNullOrWhiteSpace(grid)) builder.AddField("Grid", grid, true);

            return builder;
        }
    }

    [Kind(ItemKind.ModificationBarrel)]
    public class BarrelItem : ModificationItem
    {
        public float Length { get; set; }
        public float Velocity { get; set; }
        public bool Supressor { get; set; }

        public override EmbedBuilder ToEmbedBuilder()
        {
            var builder = base.ToEmbedBuilder();

            builder.AddField("Length", $"{Length} mm", true);
            builder.AddField("Supresses", Supressor ? "Yes" : "No", true);

            if (Velocity != 0) builder.AddField("Velocity", Velocity.ToString("+0.00;-#.00"), true);

            return builder;
        }
    }

    [Kind(ItemKind.ModificationBipod)]
    public class BipodItem : ModificationItem
    {
    }

    [Kind(ItemKind.ModificationCharge)]
    public class ChargeItem : ModificationItem
    {
    }

    [Kind(ItemKind.ModificationDevice)]
    public class DeviceItem : ModificationItem
    {
        public string Type { get; set; }
        public List<string> Modes { get; set; }

        public override EmbedBuilder ToEmbedBuilder()
        {
            var builder = base.ToEmbedBuilder();

            builder.AddField("Type", Type.Transform(To.TitleCase), true);
            builder.AddField("Modes", Modes.Humanize(x => x.Transform(To.TitleCase)), true);

            return builder;
        }
    }

    [Kind(ItemKind.ModificationForegrip)]
    public class ForegripItem : ModificationItem
    {
    }

    [Kind(ItemKind.ModificationGasblock)]
    public class GasblockItem : ModificationItem
    {
    }

    [Kind(ItemKind.ModificationHandguard)]
    public class HandguardItem : ModificationItem
    {
    }

    [Kind(ItemKind.ModificationLauncher)]
    public class LauncherItem : ModificationItem
    {
        public string Caliber { get; set; }

        public override EmbedBuilder ToEmbedBuilder()
            => base.ToEmbedBuilder().AddField("Caliber", Caliber, true);
    }

    [Kind(ItemKind.ModificationMount)]
    public class MountItem : ModificationItem
    {
    }

    [Kind(ItemKind.ModificationMuzzle)]
    public class MuzzleItem : ModificationItem
    {
        public string Type { get; set; }
        public float Velocity { get; set; }
        public float Loudness { get; set; }

        public override EmbedBuilder ToEmbedBuilder()
        {
            var builder = base.ToEmbedBuilder();

            builder.AddField("Type", Type.Transform(To.TitleCase), true);
            builder.AddField("Loudness", Loudness.ToString("+0.00;-#.00"), true);

            if (Velocity != 0) builder.AddField("Velocity", Velocity.ToString("+0.00;-#.00"), true);

            return builder;
        }
    }

    [Kind(ItemKind.ModificationPistolgrip)]
    public class PistolgripItem : ModificationItem
    {
    }

    [Kind(ItemKind.ModificationReceiver)]
    public class ReceiverItem : ModificationItem
    {
        public float Velocity { get; set; }

        public override EmbedBuilder ToEmbedBuilder()
        {
            var builder = base.ToEmbedBuilder();

            if (Velocity != 0) builder.AddField("Velocity", Velocity.ToString("+0.00;-#.00"), true);

            return builder;
        }
    }

    [Kind(ItemKind.ModificationSight)]
    public class SightItem : ModificationItem
    {
        public string Type { get; set; }
        public List<string> Magnification { get; set; }
        public bool VariableZoom { get; set; }
        public List<int> ZeroDistances { get; set; }

        public override EmbedBuilder ToEmbedBuilder()
        {
            var builder = base.ToEmbedBuilder();

            builder.AddField("Type", Type.Transform(To.TitleCase), true);
            builder.AddField("Variable Zoom", VariableZoom ? "Yes" : "No", true);
            builder.AddField("Magnification(s)", Magnification.Humanize(), true);
            builder.AddField("Zero Distances", ZeroDistances.Humanize(x => $"{x} m."), true);

            return builder;
        }
    }

    [Kind(ItemKind.ModificationSightSpecial)]
    public class SightSpecialItem : SightItem
    {
        public List<string> Modes { get; set; }
        public Color Color { get; set; }
        public string Noise { get; set; }
    }

    [Kind(ItemKind.ModificationStock)]
    public class StockItem : ModificationItem
    {
        public bool FoldRetractable { get; set; }

        public override EmbedBuilder ToEmbedBuilder()
            => base.ToEmbedBuilder().AddField("Foldable", FoldRetractable ? "Yes" : "No", true);
    }

    [Kind(ItemKind.ModificationGoggles)]
    public class GogglesItem : ModificationItem
    {
        public string Type { get; set; }
        public List<string> Modes { get; set; }
        public Color Color { get; set; }
        public string Noise { get; set; }

        public override EmbedBuilder ToEmbedBuilder()
            => base.ToEmbedBuilder().AddField("Type", Type.Transform(To.TitleCase), true);
    }
}
