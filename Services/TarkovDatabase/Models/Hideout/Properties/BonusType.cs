namespace TarkovItemBot.Services.TarkovDatabase
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum BonusType
    {
        AdditionalSlots,
        DebuffEndDelay,
        EnergyRegeneration,
        ExperienceRate,
        FuelConsumption,
        HealthRegeneration,
        HydrationRegeneration,
        InsuranceReturnTime,
        MaximumEnergyReserve,
        QuestMoneyReward,
        RagfairCommission,
        ScavCooldownTimer,
        SkillGroupLevelingBoost,
        StashSize,
        Text,
        UnlockWeaponModification
    }
}
