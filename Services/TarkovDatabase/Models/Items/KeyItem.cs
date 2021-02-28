using Discord;

namespace TarkovItemBot.Services.TarkovDatabase
{
    public class KeyItem : CommonItem
    {
        public string Location { get; set; }
        public int Usages { get; set; }

        public override EmbedBuilder ToEmbedBuilder()
        {
            var builder = base.ToEmbedBuilder();

            if (Usages != 0) builder.AddField("Uses", Usages, true);

            return builder;
        }
    }
}
