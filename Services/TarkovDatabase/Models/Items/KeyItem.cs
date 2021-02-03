using Discord;
using Humanizer;

namespace TarkovItemBot.Services
{
    public class KeyItem : CommonItem
    {
        public string Location { get; set; }

        //public override EmbedBuilder ToEmbedBuilder() => 
        //    base.ToEmbedBuilder().AddField("Location", Location.Transform(To.TitleCase), true);
    }
}
