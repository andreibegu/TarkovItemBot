using Disqord;

namespace TarkovItemBot.Services.TarkovDatabase
{
    public class MeleeItem : CommonItem
    {
        public MeleeAttack Slash { get; set; }
        public MeleeAttack Stab { get; set; }

        public override LocalEmbed ToEmbed()
        {
            var embed = base.ToEmbed();

            embed.AddField("Slash", $"`{Slash.Damage}` damage / `{Slash.Consumption}` energy consumption / `{Slash.Range}` m. range", false);
            embed.AddField("Stab", $"`{Stab.Damage}` damage / `{Stab.Consumption}` energy consumption / `{Stab.Range}` m. range", false);

            return embed;
        }
    }
}
