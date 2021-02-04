using Discord;
using Discord.Commands;
using Humanizer;
using System.Linq;
using System.Threading.Tasks;
using TarkovItemBot.Services;

namespace TarkovItemBot.Modules
{
    public class ItemModule : ModuleBase<SocketCommandContext>
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

            await Context.Message.ReplyAsync($"Total of items: `{total}` (Updated `{updated.Humanize()}`).");
        }

        [Command("item")]
        public async Task ItemAsync([Remainder] string search)
        {
            var searchResult = (await _tarkovSearch.SearchAsync(search, 1)).FirstOrDefault();

            if (searchResult == null)
            {
                await Context.Message.ReplyAsync($"Item `{Format.Sanitize(search)}` not found!");
                return;
            }

            var item = await _tarkov.GetEmbedableItemAsync(searchResult.Id, searchResult.Kind);

            await Context.Message.ReplyAsync(embed: item.ToEmbedBuilder().Build());
        }
    }
}
