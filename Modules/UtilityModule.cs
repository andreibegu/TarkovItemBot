using Discord;
using Discord.Commands;
using Humanizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TarkovItemBot.Preconditions;
using TarkovItemBot.Services.TarkovDatabase;
using TarkovItemBot.Services.TarkovDatabaseSearch;

namespace TarkovItemBot.Modules
{
    [Name("Utility")]
    public class UtilityModule : ItemBotModuleBase
    {
        private readonly TarkovDatabaseClient _tarkov;
        private readonly TarkovSearchClient _tarkovSearch;

        public UtilityModule(TarkovDatabaseClient tarkov, TarkovSearchClient tarkovSearch)
        {
            _tarkov = tarkov;
            _tarkovSearch = tarkovSearch;
        }

        [Command("slots")]
        [Alias("attachments")]
        [Summary("Displays all slots and attachments available for an item.")]
        [Remarks("slots m4a1")]
        public async Task SlotsAsync([Summary("The item to display attachments for.")][Remainder] string query)
        {
            var result = (await _tarkovSearch.SearchAsync($"name:{query}", DocType.Item, 1)).FirstOrDefault();

            if (result == null)
            {
                await ReplyAsync("No items found for query!");
                return;
            }

            var item = await _tarkov.GetItemAsync(result.Id, result.Kind);

            if (item is not IModifiableItem modifiableItem ||
                !modifiableItem.Slots.Any())
            {
                await ReplyAsync("The base item provided is not modifiable!");
                return;
            }

            var builder = new EmbedBuilder()
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
                builder.AddField(slot.Key.Humanize(LetterCasing.Title), itemResult);
            }

            await ReplyAsync(embed: builder.Build());
        }

        [Command("compatibility")]
        [Alias("compatible", "attachable")]
        [Summary("Displays all items an item can be attached to.")]
        [Remarks("compatibility pk-06")]
        public async Task CompatibilityAsync([Summary("The item to display compatibility for.")][Remainder] string query)
        {
            var result = (await _tarkovSearch.SearchAsync($"name:{query}", DocType.Item, 1)).FirstOrDefault();

            if (result == null)
            {
                await ReplyAsync("No items found for query!");
                return;
            }

            var item = await _tarkov.GetItemAsync(result.Id, result.Kind);

            if (item is not IAttachableItem attachableItem
                || !attachableItem.Compatibility.Any())
            {
                await ReplyAsync("The base item provided is not attachable!");
                return;
            }

            var builder = new EmbedBuilder()
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
                builder.AddField(slot.Key.Humanize(LetterCasing.Title), itemResult);
            }

            await ReplyAsync(embed: builder.Build());
        }

        [Command("wiki")]
        [Alias("gamepedia")]
        [Summary("Finds the wiki page of the queried item.")]
        [Remarks("wiki m4a1")]
        public async Task WikiAsync([Summary("The item to look for")][Remainder] string query)
        {
            var result = (await _tarkovSearch.SearchAsync($"name:{query}", DocType.Item, 1)).FirstOrDefault();

            if (result == null)
            {
                await ReplyAsync("No items found for query!");
                return;
            }

            await ReplyAsync($"<https://escapefromtarkov.gamepedia.com/{HttpUtility.UrlEncode(result.Name.Replace(" ", "_"))}>");
        }

        [Command("tax")]
        [Alias("commission", "flea", "market")]
        [Summary("Returns the Flea Market tax for the item most closely matching the query.")]
        [Remarks("tax 500000 Red Keycard")]
        public async Task TaxAsync([Summary("The price the item is being put up for.")] int price,
            [Summary("The item that is being put up for sale.")][Remainder][RequireLength(3, 50)] string query)
        {
            var result = (await _tarkovSearch.SearchAsync($"name:{query}", DocType.Item, 1)).FirstOrDefault();

            if (result == null)
            {
                await ReplyAsync("No items found for query!");
                return;
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

            var builder = new EmbedBuilder()
            {
                Title = $"{item.Name} ({item.ShortName})",
                Color = item.Grid.Color,
                ThumbnailUrl = item.IconUrl,
                Description = item.Description,
                Url = item.WikiUrl
            };

            builder.AddField("Base Price", $"{item.Price:#,##0} ₽", true);
            builder.AddField("Base Tax", $"{tax:#,##0} ₽", true);
            builder.AddField("Profit", $"{price - tax:#,##0} ₽", true);

            builder.WithFooter($"{item.Kind.Humanize()} • Modified {item.Modified.Humanize()}");

            await ReplyAsync(embed: builder.Build());
        }
    }
}
