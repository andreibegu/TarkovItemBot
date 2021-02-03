using Discord;
using Discord.Commands;
using Humanizer;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TarkovItemBot.Helpers;
using TarkovItemBot.Services;

namespace TarkovItemBot.Modules
{
    public class BasicModule : ModuleBase<SocketCommandContext>
    {
        private readonly TarkovDatabaseClient _tarkov;
        private readonly TarkovSearchClient _tarkovSearch;

        public BasicModule(TarkovDatabaseClient tarkov, TarkovSearchClient tarkovSearch)
        {
            _tarkov = tarkov;
            _tarkovSearch = tarkovSearch;
        }

        [Command("ping")]
        public Task PingAsync()
            => Context.Message.ReplyAsync($"Current latency: `{Context.Client.Latency}`");

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

        [Command("locations")]
        [Alias("maps")]
        [Summary("Lists all available locations to be queried for.")]
        public async Task LocationsAsync()
        {
            var locations = await _tarkov.GetLocationsAsync();
            var names = locations.Where(x => x.Available).Select(x => Format.Code(x.Name));

            await Context.Message.ReplyAsync($"All available locations: {string.Join(", ", names)}.");
        }

        [Command("location")]
        [Alias("map")]
        [Summary("Lists information about a specific location.")]
        public async Task LocationAsync([Name("Location")][Summary("The location to search for.")] string query)
        {
            var location = (await _tarkov.GetLocationsAsync(query, 1)).FirstOrDefault();

            var embed = new EmbedBuilder()
            {
                Title = location.Name,
                Description = location.Description
            };

            embed.AddField("Players", $"{location.MinPlayers}-{location.MaxPlayers}", true);
            embed.AddField("Timer", $"{location.EscapeTime} min.", true);
            embed.AddField("Has Insurance", location.Insurance ? "Yes" : "No", true);

            embed.AddField("Exfils", " \u200b", false);

            foreach (var exit in location.Exits)
            {
                embed.AddField(exit.Name, $"`{exit.Chance}%` chance / `{exit.ExfilTime}` sec. timer", true);
            }

            embed.WithFooter($"Updated {location.Modified.Humanize()}");

            await Context.Message.ReplyAsync(embed: embed.Build());
        }
    }
}
