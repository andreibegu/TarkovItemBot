using Disqord;

namespace TarkovItemBot.Services.TarkovDatabase
{
    public class KeyItem : CommonItem
    {
        public string Location { get; set; }
        public int Usages { get; set; }

        public override LocalEmbed ToEmbed()
        {
            var embed = base.ToEmbed();

            if (Usages != 0) embed.AddField("Uses", Usages, true);

            return embed;
        }
    }
}
