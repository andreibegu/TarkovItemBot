using Discord;
using Humanizer;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using TarkovItemBot.Helpers;

namespace TarkovItemBot.Services
{
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

            builder.AddGridModifier(GridModifier);

            return builder;
        }
    }

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

    public class BipodItem : ModificationItem
    {
    }

    public class ChargeItem : ModificationItem
    {
    }

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

    public class ForegripItem : ModificationItem
    {
    }

    public class GasblockItem : ModificationItem
    {
    }

    public class HandguardItem : ModificationItem
    {
    }

    public class LauncherItem : ModificationItem
    {
        public string Caliber { get; set; }

        public override EmbedBuilder ToEmbedBuilder()
            => base.ToEmbedBuilder().AddField("Caliber", Caliber, true);
    }

    public class MountItem : ModificationItem
    {
    }

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

    public class PistolgripItem : ModificationItem
    {
    }

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

    public class SightSpecialItem : SightItem
    {
        public List<string> Modes { get; set; }
        public Color Color { get; set; }
        public string Noise { get; set; }
    }

    public class StockItem : ModificationItem
    {
        public bool FoldRetractable { get; set; }

        public override EmbedBuilder ToEmbedBuilder()
            => base.ToEmbedBuilder().AddField("Foldable", FoldRetractable ? "Yes" : "No", true);
    }

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
