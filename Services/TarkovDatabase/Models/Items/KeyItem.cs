using Discord;

namespace TarkovItemBot.Services
{
    public class KeyItem : BaseItem
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
