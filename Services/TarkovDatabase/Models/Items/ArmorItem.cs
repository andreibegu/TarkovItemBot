﻿using Disqord;
using Humanizer;
using System.Collections.Generic;
using TarkovItemBot.Helpers;

namespace TarkovItemBot.Services.TarkovDatabase
{
    public class ArmorItem : CommonItem, IModifiableItem, IAttachableItem
    {
        public ArmorType Type { get; set; }
        public ArmorProperties Armor { get; set; }
        public Penalties Penalties { get; set; }
        public RicochetChance RicochetChance { get; set; }
        public IReadOnlyCollection<string> Blocking { get; set; }
        public IReadOnlyDictionary<ItemKind, IReadOnlyList<string>> Compatibility { get; set; }
        public IReadOnlyDictionary<string, Slot> Slots { get; set; }

        public override LocalEmbed ToEmbed()
        {
            var builder = base.ToEmbed();

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
