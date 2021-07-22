using Disqord;
using Humanizer;
using TarkovItemBot.Helpers;

namespace TarkovItemBot.Services.TarkovDatabase
{
    public class MedicalItem : CommonItem
    {
        public string Type { get; set; }
        public int Resources { get; set; }
        public int ResourceRate { get; set; }
        public float UseTime { get; set; }
        public Effects Effects { get; set; }

        public override LocalEmbed ToEmbed()
        {
            var embed = base.ToEmbed();

            embed.AddField("Type", Type.Transform(To.TitleCase), true);
            if (Resources != 0) embed.AddField("Resources", ResourceRate == 0 ? Resources : $"{Resources} / {ResourceRate}", true);
            embed.AddField("Use time", $"{UseTime} sec.", true);

            embed.AddEffects(Effects);

            return embed;
        }
    }
}
