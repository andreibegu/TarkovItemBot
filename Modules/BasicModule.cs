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

        public BasicModule(TarkovDatabaseClient tarkov)
        {
            _tarkov = tarkov;
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
        public async Task ItemAsync(string id)
        {
            // TODO: search and decide kind auto
            var item = await _tarkov.GetItemAsync<CommonItem>(id);

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
            embed.AddField("Has Insurance", location.Insurance ? "Yes" : "No");

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
