using Disqord;
using Disqord.Bot;
using Disqord.Bot.Commands;
using Disqord.Bot.Commands.Application;
using Humanizer;
using Qmmands;
using System.Linq;
using System.Threading.Tasks;
using TarkovItemBot.Services.TarkovDatabase;

namespace TarkovItemBot.Modules
{
    [Name("Location")]
    public class LocationModule : DiscordApplicationModuleBase
    {
        private readonly TarkovDatabaseClient _tarkov;

        public LocationModule(TarkovDatabaseClient tarkov)
        {
            _tarkov = tarkov;
        }

        [SlashCommand("locations")]
        [Description("Lists all available locations to be queried for.")]
        [RateLimit(10, 1, RateLimitMeasure.Minutes, RateLimitBucketType.User)]
        public async Task<IResult> LocationsAsync()
        {
            var locations = await _tarkov.GetLocationsAsync();
            var names = locations.Select(x => Markdown.Code(x.Name));

            return Response($"All available locations: {string.Join(", ", names)}.");
        }

        [SlashCommand("location")]
        [Description("Lists information about a specific location.")]
        [RateLimit(10, 1, RateLimitMeasure.Minutes, RateLimitBucketType.User)]
        public async Task<IResult> LocationAsync(
            [Range(3, 32)][Description("The location to look for.")] string query)
        {
            var location = (await _tarkov.GetLocationsAsync(1, query)).FirstOrDefault();

            if (location == null)
            {
                return Response("No locations found for query!");
            }

            var embed = new LocalEmbed()
            {
                Title = location.Name,
                Color = new Disqord.Color(0x968867),
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

            return Response(embed);
        }
    }
}
