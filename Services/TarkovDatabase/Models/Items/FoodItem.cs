using Disqord;
using Humanizer;
using TarkovItemBot.Helpers;

namespace TarkovItemBot.Services.TarkovDatabase
{
    public class FoodItem : CommonItem
    {
        public string Type { get; set; }
        public int Resources { get; set; }
        public float UseTime { get; set; }
        public Effects Effects { get; set; }

        public override LocalEmbed ToEmbed()
        {
            var embed = base.ToEmbed();

            embed.AddField("Type", Type.Transform(To.TitleCase), true);
            if (Resources != 0) embed.AddField("Resources", Resources, true);
            embed.AddField("Use time", $"{UseTime} sec.", true);

            embed.AddEffects(Effects);

            return embed;
        }
    }
}
