using Discord;
using Humanizer;

namespace TarkovItemBot.Services
{
    public class KeyItem : BaseItem
    {
        public string Location { get; set; }

        //public override EmbedBuilder ToEmbedBuilder() => 
        //    base.ToEmbedBuilder().AddField("Location", Location.Transform(To.TitleCase), true);
    }
}
