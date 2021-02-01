using Discord;
using Humanizer;
using System.Collections.Generic;
using TarkovItemBot.Helpers;

namespace TarkovItemBot.Services
{
    [Kind(ItemKind.Armor)]
    public class ArmorItem : CommonItem
    {
        public ArmorType Type { get; set; }
        public ArmorProperties Armor { get; set; }
        public Penalties Penalties { get; set; }
        public List<string> Blocking { get; set; }
        public Dictionary<string, Slot> Slots { get; set; }
        public object Compatibility { get; set; }

        public override EmbedBuilder ToEmbedBuilder()
        {
            var builder = base.ToEmbedBuilder();

            builder.AddField("Type", Type.Humanize(), true);
            builder.AddField("Class", Armor.Class, true);
            builder.AddField("Durability", Armor.Durability, true);
            builder.AddField("Zones", Armor.Zones.Humanize( x => x.Transform(To.TitleCase)), true);
            builder.AddField("Material", Armor.Material.Name.Transform(To.TitleCase), true);
            
            
            if (Blocking.Count != 0) builder.AddField("Blocking", Blocking.Humanize(x => x.Transform(To.TitleCase)), true);

            if (Penalties.Speed !=0 ) builder.AddField("Speed Penalty", $"{Penalties.Speed}%", true);
            if (Penalties.Mouse != 0) builder.AddField("Turning Penalty", $"{Penalties.Mouse}%", true);
            if (Penalties.Deafness != Deafness.None) builder.AddField("Deafness", Penalties.Deafness.Humanize(), true);

            return builder;
        }
    }
}
