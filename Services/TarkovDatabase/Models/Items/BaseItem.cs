
using Discord;
using Humanizer;
using System;
using System.Text.Json.Serialization;
using System.Web;
using TarkovItemBot.Helpers;

namespace TarkovItemBot.Services.TarkovDatabase
{
    public class BaseItem : IEmbedableItem
    {
        [JsonPropertyName("_id")]
        public string Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public float Weight { get; set; }
        public int MaxStack { get; set; }
        // public Rarity Rarity { get; set; }
        public Grid Grid { get; set; }
        [JsonPropertyName("_modified")]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime Modified { get; set; }
        [JsonPropertyName("_kind")]
        public ItemKind Kind { get; set; }
        [JsonIgnore]
        public string IconUrl => $"https://static.tarkov-database.com/image/icon/1-1/{Id}.png";
        public string WikiUrl => $"https://escapefromtarkov.gamepedia.com/{HttpUtility.UrlEncode(Name.Replace(" ", "_"))}";

        public virtual EmbedBuilder ToEmbedBuilder()
        {
            var embed = new EmbedBuilder()
            {
                Title = $"{Name} ({ShortName})",
                Description = Description,
                ThumbnailUrl = IconUrl,
                Color = Grid.Color,
                Url = WikiUrl
            };

            embed.AddField("Weight", $"{Weight} kg", true);
            // embed.AddField("Rarity", Rarity.Humanize().Transform(To.TitleCase), true);

            var width = Grid.Width;
            var height = Grid.Height;
            embed.AddField("Size", $"{width}x{height} ({width * height})", true);

            embed.AddField("Base Price", $"{Price:#,##0} ₽", true);

            embed.WithFooter($"{Kind.Humanize()} • Modified {Modified.Humanize()}");

            return embed;
        }
    }
}
