﻿using Disqord;
using Disqord.Bot;
using Humanizer;
using Qmmands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TarkovItemBot.Services.TarkovDatabase;
using TarkovItemBot.Services.TarkovDatabaseSearch;
using TarkovItemBot.Services.TarkovTools;

namespace TarkovItemBot.Modules
{
    [Name("Utility")]
    public class UtilityModule : DiscordModuleBase
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

        [Command("slots", "attachments")]
        [Description("Displays all slots and attachments available for an item.")]
        [Cooldown(5, 1, CooldownMeasure.Minutes, CooldownBucketType.User)]
        [Remarks("slots m4a1")]
        public async Task<DiscordCommandResult> SlotsAsync(
            [Remainder][Range(3, 100, true, true)][Description("The item to display attachments for.")] string query)
        {
            var result = (await _tarkovSearch.SearchAsync($"name:{query}", DocType.Item, 1)).FirstOrDefault();

            if (result == null)
            {
                return Reply("No items found for query!");
            }

            var item = await _tarkov.GetItemAsync(result.Id, result.Kind);

            if (item is not IModifiableItem modifiableItem ||
                !modifiableItem.Slots.Any())
            {
                return Reply("The base item provided is not modifiable!");
            }

            var embed = new LocalEmbed()
            {
                Title = $"{item.Name} ({item.ShortName})",
                Color = item.Grid.Color,
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

            return Reply(embed);
        }

        [Command("compatibility", "compatible", "attachable")]
        [Description("Displays all items an item can be attached to.")]
        [Cooldown(5, 1, CooldownMeasure.Minutes, CooldownBucketType.User)]
        [Remarks("compatibility pk-06")]
        public async Task<DiscordCommandResult> CompatibilityAsync(
            [Remainder][Range(3, 100, true, true)][Description("The item to display compatibility for.")] string query)
        {
            var result = (await _tarkovSearch.SearchAsync($"name:{query}", DocType.Item, 1)).FirstOrDefault();

            if (result == null)
            {
                return Reply("No items found for query!");
            }

            var item = await _tarkov.GetItemAsync(result.Id, result.Kind);

            if (item is not IAttachableItem attachableItem
                || !attachableItem.Compatibility.Any())
            {
                return Reply("The base item provided is not attachable!");
            }

            var embed = new LocalEmbed()
            {
                Title = $"{item.Name} ({item.ShortName})",
                Color = item.Grid.Color,
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

            return Reply(embed);
        }

        [Command("wiki", "gamepedia")]
        [Description("Finds the wiki page of the queried item.")]
        [Cooldown(10, 1, CooldownMeasure.Minutes, CooldownBucketType.User)]
        [Remarks("wiki m4a1")]
        public async Task<DiscordCommandResult> WikiAsync(
            [Remainder][Range(3, 100, true, true)][Description("The item to look for")] string query)
        {
            var result = (await _tarkovSearch.SearchAsync($"name:{query}", DocType.Item, 1)).FirstOrDefault();

            if (result == null)
            {
                return Reply("No items found for query!");
            }

            return Reply($"<https://escapefromtarkov.gamepedia.com/{HttpUtility.UrlEncode(result.Name.Replace(" ", "_"))}>");
        }

        [Command("tax", "commission", "fleatax", "markettax")]
        [Description("Returns the Flea Market tax for the item most closely matching the query.")]
        [Cooldown(5, 1, CooldownMeasure.Minutes, CooldownBucketType.User)]
        [Remarks("tax 500000 Red Keycard")]
        public async Task<DiscordCommandResult> TaxAsync(
            [Description("The price the item is being put up for.")] int price,
            [Remainder][Range(3, 100, true, true)][Description("The item that is being put up for sale.")] string query)
        {
            var result = (await _tarkovSearch.SearchAsync($"name:{query}", DocType.Item, 1)).FirstOrDefault();

            if (result == null)
            {
                return Reply("No items found for query!");
            }

            var item = await _tarkov.GetItemAsync(result.Id, result.Kind);

            var offerValue = item.Price;
            var requestValue = Convert.ToDouble(price);

            var offerModifier = Math.Log10(offerValue / requestValue);
            var requestModifier = Math.Log10(requestValue / offerValue);

            if (requestValue >= offerValue)
            {
                requestModifier = Math.Pow(requestModifier, 1.08);
            }
            else
            {
                offerModifier = Math.Pow(offerModifier, 1.08);
            }

            var tax = offerValue * 0.05 * Math.Pow(4, offerModifier) + requestValue * 0.05 * Math.Pow(4, requestModifier);

            var embed = new LocalEmbed()
            {
                Title = $"{item.Name} ({item.ShortName})",
                Color = item.Grid.Color,
                ThumbnailUrl = item.IconUrl,
                Description = item.Description,
                Url = item.WikiUrl
            };

            embed.AddField("Base Price", $"{item.Price:#,##0} ₽", true);
            embed.AddField("Base Tax", $"{tax:#,##0} ₽", true);
            embed.AddField("Profit", $"{price - tax:#,##0} ₽", true);

            embed.WithFooter($"{item.Kind.Humanize()} • Modified {item.Modified.Humanize()}");

            return Reply(embed);
        }

        [Command("pricecheck", "price", "pc", "flea", "market", "fleamarket")]
        [Description("Displays current and past prices of an item on the player-driven market.")]
        [Cooldown(5, 1, CooldownMeasure.Minutes, CooldownBucketType.User)]
        [Remarks("price Red Keycard")]
        public async Task<DiscordCommandResult> PriceCheckAsync(
            [Remainder][Range(3, 100, true, true)][Description("The item to display prices for.")] string query)
        {
            var result = (await _tarkovSearch.SearchAsync($"name:{query}", DocType.Item, 1)).FirstOrDefault();

            if (result == null)
            {
                return Reply("No items found for query!");
            }

            var priceData = await _tarkovTools.GetItemPriceDataAsync(result.Id);

            if(priceData.Avg24hPrice == 0)
            {
                return Reply($"No price data found for item `{result.Name}`!");
            }

            var item = await _tarkov.GetItemAsync(result.Id, result.Kind);

            var embed = new LocalEmbed()
            {
                Title = $"{item.Name} ({item.ShortName})",
                Color = item.Grid.Color,
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

            return Reply(embed);
        }
    }
}
