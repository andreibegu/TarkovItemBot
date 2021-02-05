using Discord;
using Discord.Commands;
using Humanizer;
using System.Linq;
using System.Threading.Tasks;
using TarkovItemBot.Services;

namespace TarkovItemBot.Modules
{
    public class LocationModule : ModuleBase<SocketCommandContext>
    {
        private readonly TarkovDatabaseClient _tarkov;

        public LocationModule(TarkovDatabaseClient tarkov)
        {
            _tarkov = tarkov;
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

            if(location.Exits != null)
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
