using Discord;
using Humanizer;
using System.Collections.Generic;
using TarkovItemBot.Helpers;

namespace TarkovItemBot.Services
{
    public class ArmorItem : BaseItem
    {
        public ArmorType Type { get; set; }
        public ArmorProperties Armor { get; set; }
        public Penalties Penalties { get; set; }
        public IReadOnlyCollection<string> Blocking { get; set; }
        public Dictionary<string, Slot> Slots { get; set; }
        public object Compatibility { get; set; }
        public RicochetChance RicochetChance { get; set; }

        public override EmbedBuilder ToEmbedBuilder()
        {
            var builder = base.ToEmbedBuilder();

            builder.AddField("Type", Type.Humanize(), true);

            builder.AddArmorProperties(Armor);

            if (Blocking.Count != 0) builder.AddField("Blocking", Blocking.Humanize(x => x.Transform(To.TitleCase)), true);
            if (RicochetChance != RicochetChance.None) builder.AddField("Ricochet Chance", RicochetChance.Humanize(), true);

            if (Penalties.Speed != 0) builder.AddField("Speed Penalty", $"{Penalties.Speed}%", true);
            if (Penalties.Mouse != 0) builder.AddField("Turning Penalty", $"{Penalties.Mouse}%", true);
            if (Penalties.Deafness != Deafness.None) builder.AddField("Deafness", Penalties.Deafness.Humanize(), true);

            return builder;
        }
    }
}
