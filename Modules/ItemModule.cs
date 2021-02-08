using Discord;
using Discord.Commands;
using Humanizer;
using System;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task TotalAsync(ItemKind kind = ItemKind.None)
        {
            var info = await _tarkov.GetItemsInfoAsync();

            int total = kind == ItemKind.None ? info.Total : info.Kinds[kind].Count;
            var updated = kind == ItemKind.None ? info.Modified : info.Kinds[kind].Modified;

            await ReplyAsync($"Total of items: `{total}` (Updated `{updated.Humanize()}`).");
        }

        [Command("item")]
        public async Task ItemAsync([Remainder] string query)
        {
            if (query.Length < 3 || query.Length > 50)
            {
                await Context.Message.ReplyAsync($"Query must be 3-50 characters long!");
                return;
            }

            var result = (await _tarkovSearch.SearchAsync(query, 1)).FirstOrDefault();

            if (result == null)
            {
                await Context.Message.ReplyAsync("No items found for query!");
                return;
            }

            var item = await _tarkov.GetEmbedableItemAsync(result.Id, result.Kind);
            await ReplyAsync(embed: item.ToEmbedBuilder().Build());
        }

        [Command("tax")]
        public async Task TaxAsync(int price, [Remainder] string query)
        {
            if (query.Length < 3 || query.Length > 50)
            {
                await Context.Message.ReplyAsync($"Query must be 3-50 characters long!");
                return;
            }

            var result = (await _tarkovSearch.SearchAsync(query, 1)).FirstOrDefault();

            if (result == null)
            {
                await Context.Message.ReplyAsync("No items found for query!");
                return;
            }

            var item = await _tarkov.GetEmbedableItemAsync(result.Id, result.Kind) as BaseItem;

            var offerValue = item.Price;
            var requestValue = price;

            var offerModifier = Math.Log10(offerValue / requestValue);
            offerModifier = requestValue < offerValue ? Math.Pow(offerModifier, 1.08) : offerModifier;

            var requestModifier = Math.Log10(requestValue/ offerValue);
            requestModifier = requestValue >= offerValue ? Math.Pow(requestModifier, 1.08) : requestModifier;

            var tax = offerValue * 0.05 * Math.Pow(4, offerModifier) + requestValue * 0.05 * Math.Pow(4, requestModifier);

            var builder = new EmbedBuilder()
            {
                Title = $"{item.Name} ({item.ShortName})",
                Color = item.Grid.Color,
                ThumbnailUrl = item.IconUrl,
                Description = item.Description
            }.AddField("Base Price", $"{item.Price:#,##0} ₽", true)
            .AddField("Tax", $"{tax:#,##0} ₽", true);

            await ReplyAsync(embed: builder.Build());
        }
    }
}
