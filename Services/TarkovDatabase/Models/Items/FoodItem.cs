using Discord;
using Humanizer;
using TarkovItemBot.Helpers;

namespace TarkovItemBot.Services
{
    public class FoodItem : BaseItem
    {
        public string Type { get; set; }
        public int Resources { get; set; }
        public float UseTime { get; set; }
        public Effects Effects { get; set; }

        public override EmbedBuilder ToEmbedBuilder()
        {
            var builder = base.ToEmbedBuilder();

            builder.AddField("Type", Type.Transform(To.TitleCase), true);
            builder.AddField("Resources", Resources, true);
            builder.AddField("Use time", $"{UseTime} sec.", true);

            builder.AddEffects(Effects);

            return builder;
        }
    }
}
