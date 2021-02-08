using Discord;
using Discord.Commands;
using Humanizer;
using System.Linq;
using System.Threading.Tasks;
using TarkovItemBot.Services;

namespace TarkovItemBot.Modules
{
    [Name("Location")]
    public class LocationModule : ItemBotModuleBase
    {
        private readonly TarkovDatabaseClient _tarkov;

        public LocationModule(TarkovDatabaseClient tarkov)
        {
            _tarkov = tarkov;
        }

        [Command("locations")]
        [Alias("maps")]
        [Summary("Lists all available locations to be queried for.")]
        [Remarks("locations")]
        public async Task LocationsAsync()
        {
            var locations = await _tarkov.GetLocationsAsync();
            var names = locations.Select(x => Format.Code(x.Name));

            await Context.Message.ReplyAsync($"All available locations: {string.Join(", ", names)}.");
        }

        [Command("location")]
        [Alias("map", "m", "l")]
        [Summary("Lists information about a specific location.")]
        [Remarks("location The Lab")]
        public async Task LocationAsync([Remainder] string query)
        {
            if (query.Length < 3 || query.Length > 50)
            {
                await Context.Message.ReplyAsync($"Query must be 3-50 characters long!");
                return;
            }

            var location = (await _tarkov.GetLocationsAsync(query, 1)).FirstOrDefault();

            if (location == null)
            {
                await Context.Message.ReplyAsync("No locations found for query!");
                return;
            }

            var embed = new EmbedBuilder()
            {
                Title = location.Name,
                Color = new Discord.Color(0x968867),
                Description = location.Description
            };

            embed.AddField("Players", $"{location.MinPlayers}-{location.MaxPlayers}", true);
            embed.AddField("Timer", $"{location.EscapeTime} min.", true);
            embed.AddField("Has Insurance", location.Insurance ? "Yes" : "No", true);

            if (location.Exits != null)
            {
                embed.AddField("Exfils", " \u200b", false);

                foreach (var exit in location.Exits)
                {
                    embed.AddField(exit.Name, $"`{exit.Chance}%` chance / `{exit.ExfilTime}` sec. timer", true);
                }
            }

            embed.WithFooter($"Updated {location.Modified.Humanize()}");

            await Context.Message.ReplyAsync(embed: embed.Build());
        }
    }
}
