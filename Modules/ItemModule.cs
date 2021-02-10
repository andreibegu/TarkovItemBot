using Discord;
using Discord.Commands;
using Humanizer;
using System;
using System.Linq;
using System.Threading.Tasks;
using TarkovItemBot.Preconditions;
using TarkovItemBot.Services;

namespace TarkovItemBot.Modules
{
    [Name("Item")]
    public class ItemModule : ItemBotModuleBase
    {
        private readonly TarkovDatabaseClient _tarkov;
        private readonly TarkovSearchClient _tarkovSearch;

        public ItemModule(TarkovDatabaseClient tarkov, TarkovSearchClient tarkovSearch)
        {
            _tarkov = tarkov;
            _tarkovSearch = tarkovSearch;
        }

        [Command("total")]
        [Alias("t")]
        [Summary("Returns the total items of a kind.")]
        [Remarks("total Ammunition")]
        public async Task TotalAsync(ItemKind kind = ItemKind.None)
        {
            var info = await _tarkov.GetItemsInfoAsync();

            int total = kind == ItemKind.None ? info.Total : info.Kinds[kind].Count;
            var updated = kind == ItemKind.None ? info.Modified : info.Kinds[kind].Modified;

            await ReplyAsync($"Total of items: `{total}` (Updated `{updated.Humanize()}`).");
        }

        [Command("item")]
        [Alias("i", "it")]
        [Summary("Returns detailed information for the item most closely matching the query.")]
        [Remarks("item Zagustin")]
        public async Task ItemAsync([Remainder][RequireLength(3, 50)] string query)
        {
            var result = (await _tarkovSearch.SearchAsync($"name:{query}", 1)).FirstOrDefault();

            if (result == null)
            {
                await ReplyAsync("No items found for query!");
                return;
            }

            var item = await _tarkov.GetEmbedableItemAsync(result.Id, result.Kind);
            await ReplyAsync(embed: item.ToEmbedBuilder().Build());
        }

        [Command("tax")]
        [Alias("commission", "flea", "market")]
        [Summary("Returns the Flea Market tax for the item most closely matching the query.")]
        [Remarks("tax 500000 Red Keycard")]
        public async Task TaxAsync(int price, [Remainder][RequireLength(3, 50)] string query)
        {
            var result = (await _tarkovSearch.SearchAsync($"name:{query}", 1)).FirstOrDefault();

            if (result == null)
            {
                await ReplyAsync("No items found for query!");
                return;
            }

            var item = await _tarkov.GetEmbedableItemAsync(result.Id, result.Kind) as BaseItem;

            var offerValue = item.Price;
            var requestValue = price;

            var offerModifier = Math.Log10(offerValue / requestValue);
            offerModifier = requestValue < offerValue ? Math.Pow(offerModifier, 1.08) : offerModifier;

            var requestModifier = Math.Log10(requestValue / offerValue);
            requestModifier = requestValue >= offerValue ? Math.Pow(requestModifier, 1.08) : requestModifier;

            var tax = offerValue * 0.05 * Math.Pow(4, offerModifier) + requestValue * 0.05 * Math.Pow(4, requestModifier);

            var builder = new EmbedBuilder()
            {
                Title = $"{item.Name} ({item.ShortName})",
                Color = item.Grid.Color,
                ThumbnailUrl = item.IconUrl,
                Description = item.Description
            };

            builder.AddField("Base Price", $"{item.Price:#,##0} ₽", true);
            builder.AddField("Tax", $"{tax:#,##0} ₽", true);
            builder.AddField("Profit", $"{price - tax:#,##0} ₽", true);

            builder.WithFooter($"{item.Kind.Humanize()} • Modified {item.Modified.Humanize()}");

            await ReplyAsync(embed: builder.Build());
        }
    }
}
