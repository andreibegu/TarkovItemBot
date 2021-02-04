using Discord;
using Humanizer;
using System.Collections.Generic;
using System.Linq;

namespace TarkovItemBot.Services
{
    class TacticalrigItem : BaseItem
    {
        public List<ContainerGrid> Grids { get; set; }
        public Penalties Penalties { get; set; }
        public ArmorProperties Armor { get; set; }

        public override EmbedBuilder ToEmbedBuilder()
        {
            var builder = base.ToEmbedBuilder();

            if (Armor != null)
            {
                builder.AddField("Class", Armor.Class, true);
                builder.AddField("Durability", Armor.Durability, true);
                builder.AddField("Zones", Armor.Zones.Humanize(x => x.Transform(To.TitleCase)), true);
                builder.AddField("Material", Armor.Material.Name.Transform(To.TitleCase), true);
            }

            if (Penalties.Speed != 0) builder.AddField("Speed Penalty", $"{Penalties.Speed}%", true);
            if (Penalties.Mouse != 0) builder.AddField("Turning Penalty", $"{Penalties.Mouse}%", true);

            builder.AddField("Grids", $"`{Grids.Count}` {"grid".ToQuantity(Grids.Count, ShowQuantityAs.None)}," +
                $"`{Grids.Sum(x => x.Width * x.Height)}` slots total", true);

            return builder;
        }
    }
}
