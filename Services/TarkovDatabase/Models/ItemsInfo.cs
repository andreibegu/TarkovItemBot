namespace TarkovItemBot.Services
{
    public record ItemsInfo(int Total, int Modified, KindsInfo Kinds);

    public record KindsInfo(KindInfo Ammunition, KindInfo Armor, KindInfo Backpack, KindInfo Barter, KindInfo Clothing,
        KindInfo Common, KindInfo Container, KindInfo Firearm, KindInfo Food, KindInfo Grenade, KindInfo Headphone,
        KindInfo Key, KindInfo Magazine, KindInfo Map, KindInfo Medical, KindInfo Melee, KindInfo Modification,
        KindInfo ModificationBarrel, KindInfo ModificationBipod, KindInfo ModificationCharge, KindInfo ModificationDevice,
        KindInfo ModificationForegrip, KindInfo ModificationGasblock, KindInfo ModificationGoggles, KindInfo ModificationHandguard,
        KindInfo ModificationLauncher, KindInfo ModificationMount, KindInfo ModificationMuzzle, KindInfo ModificationPistolgrip,
        KindInfo ModificationReceiver, KindInfo ModificationSight, KindInfo ModificationSightSpecial, KindInfo ModificationStock,
        KindInfo Money, KindInfo Tacticalrig);

    public record KindInfo(int Count, int Modified);

}
