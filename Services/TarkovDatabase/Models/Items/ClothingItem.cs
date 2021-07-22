using Disqord;
using Humanizer;
using System.Collections.Generic;

namespace TarkovItemBot.Services.TarkovDatabase
{
    public class ClothingItem : CommonItem, IModifiableItem
    {
        public IReadOnlyCollection<string> Blocking { get; set; }
        public Penalties Penalties { get; set; }
        public string Type { get; set; }
        public IReadOnlyDictionary<string, Slot> Slots { get; set; }

        public override LocalEmbed ToEmbed()
        {
            var embed = base.ToEmbed();

            embed.AddField("Type", Type.Transform(To.TitleCase), true);

            if (Blocking.Count != 0) embed.AddField("Blocking", Blocking.Humanize(x => x.Transform(To.TitleCase)), true);

            if (Penalties.Speed != 0) embed.AddField("Speed Penalty", $"{Penalties.Speed}%", true);
            if (Penalties.Mouse != 0) embed.AddField("Turning Penalty", $"{Penalties.Mouse}%", true);
            if (Penalties.Deafness != Deafness.None) embed.AddField("Deafness", Penalties.Deafness.Humanize(), true);

            return embed;
        }
    }
}
