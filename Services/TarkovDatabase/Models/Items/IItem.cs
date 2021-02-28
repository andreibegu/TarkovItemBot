using Discord;
using System;

namespace TarkovItemBot.Services.TarkovDatabase
{
    public interface IItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public float Weight { get; set; }
        public int MaxStack { get; set; }
        public Grid Grid { get; set; }
        public DateTime Modified { get; set; }
        public ItemKind Kind { get; set; }
        public string IconUrl { get; }
        public string WikiUrl { get; }
        public EmbedBuilder ToEmbedBuilder();
    }
}
