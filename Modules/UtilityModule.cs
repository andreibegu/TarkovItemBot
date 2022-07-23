using Disqord;
using Disqord.Bot;
using Disqord.Bot.Commands;
using Disqord.Bot.Commands.Application;
using Humanizer;
using Qmmands;
using Qommon;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TarkovItemBot.Services.TarkovDatabase;
using TarkovItemBot.Services.TarkovDatabaseSearch;
using TarkovItemBot.Services.TarkovTools;
using Color = Disqord.Color;

namespace TarkovItemBot.Modules
{
    [Name("Utility")]
    public class UtilityModule : DiscordApplicationModuleBase
    {
        private readonly TarkovDatabaseClient _tarkov;
        private readonly TarkovSearchClient _tarkovSearch;
        private readonly TarkovToolsClient _tarkovTools;

        public UtilityModule(TarkovDatabaseClient tarkov, TarkovSearchClient tarkovSearch, TarkovToolsClient tarkovTools)
        {
            _tarkov = tarkov;
            _tarkovSearch = tarkovSearch;
            _tarkovTools = tarkovTools;
        }

        [SlashCommand("slots")]
        [Description("Displays all slots and attachments available for an item.")]
        [RateLimit(5, 1, RateLimitMeasure.Minutes, RateLimitBucketType.User)]
        public async Task<IResult> SlotsAsync(
            [Range(3, 100)][Description("The item to display attachments for.")] string query)
        {
            var result = (await _tarkovSearch.SearchAsync($"name:{query}", DocType.Item, 1)).FirstOrDefault();

            if (result == null)
            {
                return Response("No items found for query!");
            }

            var item = await _tarkov.GetItemAsync(result.Id, result.Kind);

            if (item is not IModifiableItem modifiableItem ||
                !modifiableItem.Slots.Any())
            {
                return Response("The base item provided is not modifiable!");
            }

            var embed = new LocalEmbed()
            {
                Title = $"{item.Name} ({item.ShortName})",
                Color = new Optional<Color>(item.Grid.Color),
                ThumbnailUrl = item.IconUrl,
                Description = item.Description,
                Url = item.WikiUrl
            };

            foreach (var slot in modifiableItem.Slots)
            {
                var items = new List<IItem>();

                foreach (var filter in slot.Value.Filter)
                {
                    var filterItems = await _tarkov.GetItemsAsync(filter.Key, filter.Value);
                    items.AddRange(filterItems);
                }

                var itemResult = items.Any() ? items.Humanize(x => $"`{x.ShortName}`") : "None";
                embed.AddField(slot.Key.Humanize(LetterCasing.Title), itemResult);
            }

            return Response(embed);
        }

        [SlashCommand("compatibility")]
        [Description("Displays all items an item can be attached to.")]
        [RateLimit(5, 1, RateLimitMeasure.Minutes, RateLimitBucketType.User)]
        public async Task<IResult> CompatibilityAsync(
            [Range(3, 100)][Description("The item to display compatibility for.")] string query)
        {
            var result = (await _tarkovSearch.SearchAsync($"name:{query}", DocType.Item, 1)).FirstOrDefault();

            if (result == null)
            {
                return Response("No items found for query!");
            }

            var item = await _tarkov.GetItemAsync(result.Id, result.Kind);

            if (item is not IAttachableItem attachableItem
                || !attachableItem.Compatibility.Any())
            {
                return Response("The base item provided is not attachable!");
            }

            var embed = new LocalEmbed()
            {
                Title = $"{item.Name} ({item.ShortName})",
                Color = new Optional<Color>(item.Grid.Color),
                ThumbnailUrl = item.IconUrl,
                Description = item.Description,
                Url = item.WikiUrl
            };

            foreach (var slot in attachableItem.Compatibility)
            {
                var items = await _tarkov.GetItemsAsync(slot.Key, slot.Value);

                var itemResult = items.Any() ? items.Humanize(x => $"`{x.ShortName}`") : "None";
                embed.AddField(slot.Key.Humanize(LetterCasing.Title), itemResult);
            }

            return Response(embed);
        }

        [SlashCommand("wiki")]
        [Description("Finds the wiki page of the queried item.")]
        [RateLimit(10, 1, RateLimitMeasure.Minutes, RateLimitBucketType.User)]
        public async Task<IResult> WikiAsync(
            [Range(3, 100)][Description("The item to look for")] string query)
        {
            var result = (await _tarkovSearch.SearchAsync($"name:{query}", DocType.Item, 1)).FirstOrDefault();

            if (result == null)
            {
                return Response("No items found for query!");
            }

            return Response($"<https://escapefromtarkov.gamepedia.com/{HttpUtility.UrlEncode(result.Name.Replace(" ", "_"))}>");
        }

        [SlashCommand("pricecheck")]
        [Description("Displays current and past prices of an item on the player-driven market.")]
        [RateLimit(5, 1, RateLimitMeasure.Minutes, RateLimitBucketType.User)]
        public async Task<IResult> PriceCheckAsync(
            [Range(3, 100)][Description("The item to display prices for.")] string query)
        {
            var result = (await _tarkovSearch.SearchAsync($"name:{query}", DocType.Item, 1)).FirstOrDefault();

            if (result == null)
            {
                return Response("No items found for query!");
            }

            var priceData = await _tarkovTools.GetItemPriceDataAsync(result.Id);

            if (priceData.Avg24hPrice == 0)
            {
                return Response($"No price data found for item `{result.Name}`!");
            }

            var item = await _tarkov.GetItemAsync(result.Id, result.Kind);

            var embed = new LocalEmbed()
            {
                Title = $"{item.Name} ({item.ShortName})",
                Color = new Optional<Color>(item.Grid.Color),
                ThumbnailUrl = item.IconUrl,
                Description = item.Description,
                Url = item.WikiUrl
            };

            var size = item.Grid.Height * item.Grid.Width;

            embed.AddField("Avg 24h Price", $"{priceData.Avg24hPrice:#,##0} ₽", true);
            embed.AddField("Low 24h Price", $"{priceData.Low24hPrice:#,##0} ₽", true);
            embed.AddField("High 24h Price", $"{priceData.High24hPrice:#,##0} ₽", true);
            embed.AddField("Price per Slot", $"{priceData.Avg24hPrice / size:#,##0} ₽ ({size} slots)", true);
            embed.AddField("Daily Price Change", $"{priceData.ChangeLast48h:+0.00;-#.00}%", true);

            var maximumProfit = priceData.SellData.Where(x => x.Source != ItemPriceSource.FleaMarket)
                .MaxBy(x => x.Price);

            embed.AddField("Maximum trader profit", $"Sell to {maximumProfit.Source.Humanize()} " +
                $"for {maximumProfit.Price:#,##0} {maximumProfit.Currency.Humanize()}" +
                $" ({maximumProfit.Price/size:#,##0} {maximumProfit.Currency.Humanize()} per slot)");

            return Response(embed);
        }
    }
}
