using Discord;

namespace TarkovItemBot.Services
{
    [Kind(ItemKind.Melee)]
    public class MeleeItem : CommonItem
    {
        public MeleeAttack Slash { get; set; }
        public MeleeAttack Stab { get; set; }

        public override EmbedBuilder ToEmbedBuilder()
        {
            var builder = base.ToEmbedBuilder();

            builder.AddField("Slash", $"`{Slash.Damage}` damage; `{Slash.Consumption}` energy consumption / `{Slash.Range}` m. range", false);
            builder.AddField("Stab", $"`{Stab.Damage}` damage; `{Stab.Consumption}` energy consumption / `{Stab.Range}` m. range", false);

            return builder;
        }
    }
}
