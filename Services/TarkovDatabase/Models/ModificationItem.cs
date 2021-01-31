using System.Collections.Generic;
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
    }

    [Kind(ItemKind.ModificationBarrel)]
    public class BarrelItem : ModificationItem
    {
        public float Length { get; set; }
        public float Velocity { get; set; }
        public bool Supressor { get; set; }
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
    }

    [Kind(ItemKind.ModificationPistolgrip)]
    public class PistolgripItem : ModificationItem
    {
    }

    [Kind(ItemKind.ModificationReceiver)]
    public class ReceiverItem : ModificationItem
    {
        public float Velocity { get; set; }
    }

    [Kind(ItemKind.ModificationSight)]
    public class SightItem : ModificationItem
    {
        public string Type { get; set; }
        public List<string> Magnification { get; set; }
        public bool VariableZoom { get; set; }
        public List<int> ZeroDistances { get; set; }
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
    }

    [Kind(ItemKind.ModificationGoggles)]
    public class GogglesItem : ModificationItem
    {
        public string Type { get; set; }
        public List<string> Modes { get; set; }
        public Color Color { get; set; }
        public string Noise { get; set; }
    }
}
