using Discord;
using Discord.Commands;
using Humanizer;
using System.Linq;
using System.Threading.Tasks;
using TarkovItemBot.Preconditions;
using TarkovItemBot.Services.TarkovDatabase;
using TarkovItemBot.Services.TarkovDatabaseSearch;

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
        public async Task TotalAsync([Summary("The kind of the item group.")]ItemKind kind = ItemKind.None)
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
        public async Task ItemAsync([Remainder][RequireLength(3, 50)][Summary("The item to look for.")] string query)
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
    }
}
