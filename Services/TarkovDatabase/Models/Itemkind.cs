using System.Text.Json.Serialization;

namespace TarkovItemBot.Services
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ItemKind
    {
        None,
        Ammunition,
        Armor,
        Backpack,
        Barter,
        Clothing,
        Common,
        Container,
        Firearm,
        Food,
        Grenade,
        Headphone,
        Key,
        Magazine,
        Map,
        Medical,
        Melee,
        Modification,
        ModificationBarrel,
        ModificationBipod,
        ModificationCharge,
        ModificationDevice,
        ModificationForegrip,
        ModificationGasblock,
        ModificationGoggles,
        ModificationHandguard,
        ModificationLauncher,
        ModificationMount,
        ModificationMuzzle,
        ModificationPistolgrip,
        ModificationReceiver,
        ModificationSight,
        ModificationSightSpecial,
        ModificationStock,
        Money,
        Tacticalrig
    }
}
