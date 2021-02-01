using System.Collections.Generic;

namespace TarkovItemBot.Services
{
    public record Effects(Effect Energy, Effect EnergyRate, Effect Hydration, Effect HydrationRate, Effect Stamina, Effect StaminaRate,
        Effect Health, Effect HealthRate, Effect Bloodloss, Effect LightBleeding, Effect HeavyBleeding, Effect Fracture, Effect Contusion,
        Effect Pain, Effect TunnelVision, Effect Tremor, Effect Toxication, Effect Antidote, Effect RadiationExposure, Effect BodyTemperature,
        Effect Mobility, Effect Recoil, Effect ReloadSpeed, Effect LootSpeed, Effect UnlockSpeed, Effect DestroyedPart, Effect WeightLimit,
        Effect DamageModifier, List<Effect> Skill);

    public class Effect
    {
        public string Name { get; set; }
        public int ResourceCosts { get; set; }
        public float FadeIn { get; set; }
        public float FadeOut { get; set; }
        public float Chance { get; set; }
        public float Delay { get; set; }
        public float Duration { get; set; }
        public float Value { get; set; }
        public bool IsPercent { get; set; }
        public bool Removes { get; set; }
        public EffectPenalties Penalties { get; set; }
    }
}
