using Discord;
using Humanizer;
using Qmmands;
using System.Linq;
using System.Threading.Tasks;
using TarkovItemBot.Services.Commands;
using TarkovItemBot.Services.TarkovDatabase;

namespace TarkovItemBot.Modules
{
    [Name("Location")]
    public class LocationModule : DiscordModuleBase
    {
        private readonly TarkovDatabaseClient _tarkov;

        public LocationModule(TarkovDatabaseClient tarkov)
        {
            _tarkov = tarkov;
        }

        [Command("locations", "maps")]
        [Description("Lists all available locations to be queried for.")]
        [Cooldown(10, 1, CooldownMeasure.Minutes, CooldownType.User)]
        [Remarks("locations")]
        public async Task LocationsAsync()
        {
            var locations = await _tarkov.GetLocationsAsync();
            var names = locations.Select(x => Format.Code(x.Name));

            await ReplyAsync($"All available locations: {string.Join(", ", names)}.");
        }

        [Command("location", "map", "m", "l")]
        [Description("Lists information about a specific location.")]
        [Cooldown(10, 1, CooldownMeasure.Minutes, CooldownType.User)]
        [Remarks("location The Lab")]
        public async Task LocationAsync(
            [Remainder][Range(3, 32, true, true)][Description("The location to look for.")] string query)
        {
            var location = (await _tarkov.GetLocationsAsync(1, query)).FirstOrDefault();

            if (location == null)
            {
                await ReplyAsync("No locations found for query!");
                return;
            }

            var embed = new EmbedBuilder()
            {
                Title = location.Name,
                Color = new Discord.Color(0x968867),
                Description = location.Description,
                Url = location.WikiUrl
            };

            embed.AddField("Players", $"{location.MinPlayers}-{location.MaxPlayers}", true);
            embed.AddField("Timer", $"{location.EscapeTime} min.", true);
            embed.AddField("Has Insurance", location.Insurance ? "Yes" : "No", true);

            if (location.Exits != null)
            {
                embed.AddField("Exfils", " \u200b", false);

                foreach (var exit in location.Exits)
                {
                    embed.AddField(exit.Name, $"`{exit.ExfilTime}` sec. timer\n`{exit.Chance}%` chance ", true);
                }
            }

            embed.WithFooter($"Modified {location.Modified.Humanize()}");

            await ReplyAsync(embed: embed.Build());
        }
    }
}
