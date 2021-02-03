
using Discord;
using Humanizer;
using System;
using System.Text.Json.Serialization;
using TarkovItemBot.Helpers;

namespace TarkovItemBot.Services
{
    public class CommonItem : IEmbedableItem
    {
        [JsonPropertyName("_id")]
        public string Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public float Weight { get; set; }
        public int MaxStack { get; set; }
        public string Rarity { get; set; }
        public Grid Grid { get; set; }
        [JsonPropertyName("_modified")]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime Modified { get; set; }
        [JsonPropertyName("_kind")]
        public ItemKind Kind { get; set; }
        [JsonIgnore]
        public string IconUrl => $"https://raw.githubusercontent.com/RatScanner/EfTIcons/master/uid/{Id}.png";

        public virtual EmbedBuilder ToEmbedBuilder()
        {
            var embed = new EmbedBuilder()
            {
                Title = $"{Name} ({ShortName})",
                Description = Description,
                ThumbnailUrl = IconUrl,
                Color = Grid.Color
            };

            embed.AddField("Weight", $"{Weight} kg", true);
            embed.AddField("Rarity", Rarity.FirstCharUpper(), true);

            var width = Grid.Width;
            var height = Grid.Height;
            embed.AddField("Size", $"{width}x{height} ({width * height})", true);

            embed.AddField("Base Price", $"{Price:#,##0} ₽", true);

            embed.WithFooter($"Updated {Modified.Humanize()}");

            return embed;
        }
    }
}
