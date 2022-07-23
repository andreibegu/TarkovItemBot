using ConsoleTables;
using Disqord;
using Disqord.Bot;
using Disqord.Bot.Commands;
using Disqord.Bot.Commands.Application;
using Humanizer;
using Qmmands;
using Qommon;
using System;
using System.Linq;
using System.Threading.Tasks;
using TarkovItemBot.Services.TarkovDatabase;
using TarkovItemBot.Services.TarkovDatabaseSearch;
using Color = Disqord.Color;

namespace TarkovItemBot.Modules
{
    [Name("Ballistics")]
    public class BallisticsModule : DiscordApplicationModuleBase
    {
        private readonly TarkovDatabaseClient _tarkov;
        private readonly TarkovSearchClient _tarkovSearch;

        public BallisticsModule(TarkovDatabaseClient tarkov, TarkovSearchClient tarkovSearch)
        {
            _tarkov = tarkov;
            _tarkovSearch = tarkovSearch;
        }

        [SlashCommand("rangecard")]
        [Description("Displays a range card for a specific cartridge.")]
        [RateLimit(5, 1, RateLimitMeasure.Minutes, RateLimitBucketType.User)]
        public async Task<IResult> BallisticsAsync(
            [Range(3, 100)][Description("The item to display the range card for.")] string query)
        {
            var result = (await _tarkovSearch.SearchAsync($"(name:{query}) AND kind:ammunition",
                DocType.Item, 1)).FirstOrDefault();

            if (result == null)
            {
                return Response("No items found for query!");
            }

            var item = await _tarkov.GetItemAsync<AmmunitionItem>(result.Id);

            var embed = new LocalEmbed()
            {
                Title = $"{item.Name} ({item.ShortName})",
                Color = new Optional<Color>(item.Grid.Color),
                ThumbnailUrl = item.IconUrl,
                Description = item.Description,
                Url = item.BallisticsLink
            };

            embed.AddField("Initial Damage", item.Damage, true);
            embed.AddField("Initial Penetration", item.Penetration, true);
            embed.AddField("Initial Velocity", $"{item.Velocity}m/s", true);

            var interval = item.Type == "buckshot" ? 25 : 100;
            var gte = item.Type == "buckshot" ? 25 : 100;
            var lte = item.Type == "buckshot" ? 250 : 1000;

            var statsResult = await _tarkov.GetDistanceStatisticsAsync(item.Id, gte, lte);

            if (statsResult == null)
            {
                return Response("No simulation results found for the given item!");
            }

            var stats = statsResult
                .Where(x => x.Distance % interval == 0);

            var card = new ConsoleTable("Range", "Velocity", "Damage", "Pen.", "Drop", "TOF");

            foreach (var stat in stats)
                card.AddRow($"{stat.Distance}m", $"{stat.Velocity:F2}m/s", $"{stat.Damage:F2}",
                    $"{stat.PenetrationPower:F2}", $"{stat.Drop:F2}m", $"{stat.TimeOfFlight:F2}s");

            embed.AddField("Range Card", $"```{card.ToMinimalString()}```");

            embed.AddField("Notice", $"Range card was generated given no zeroing, weapon velocity modifiers or attachments.");

            var maxModified = stats.Max(x => x.Modified);
            embed.WithFooter($"Last modified {maxModified.Humanize()}");

            return Response(embed);
        }

        [SlashCommand("armorsimulation")]
        [Description("Displays armor simulation data for a specific cartridge and armor.")]
        [RateLimit(5, 1, RateLimitMeasure.Minutes, RateLimitBucketType.User)]
        public async Task<IResult> ArmorAsync(
            [Range(3, 100)][Description("The ammo to simulate against.")] string ammo,
            [Range(3, 100)][Description("The armor to simulate against.")] string armor,
            [Description("The range to simulate at.")] int range = 20)
        {
            var ammoResult = (await _tarkovSearch.SearchAsync($"(name:{ammo}) AND kind:ammunition",
                DocType.Item, 1)).FirstOrDefault();
            var armorResult = (await _tarkovSearch.SearchAsync($"(name:{armor}) AND kind:armor",
                DocType.Item, 1)).FirstOrDefault();

            if (ammoResult == null)
            {
                return Response("No ammunition found for the query!");
            } else if (armorResult == null)
            {
                return Response("No armor found for the query!");
            }

            var ammoItem = await _tarkov.GetItemAsync<AmmunitionItem>(ammoResult.Id);
            var armorItem = await _tarkov.GetItemAsync<ArmorItem>(armorResult.Id);

            var results = await _tarkov.GetArmorStatisticsAsync(ammoItem.Id, armorItem.Id, 0, 1000);

            if (results == null)
            {
                return Response($"No simulation results found for `{ammoItem.Name}` & `{armorItem.Name}`!");
            }

            var simulation = results
                .OrderBy(x => Math.Abs(x.Distance - range)).FirstOrDefault();

            var embed = new LocalEmbed()
            {
                Title = $"{ammoItem.ShortName} vs {armorItem.ShortName}",
                Description = 
                    $"Summary of simulating the impact of `{ammoItem.Name}` against `{armorItem.Name}` at a range of `{simulation.Distance}`m. " +
                    $"Values presented are means of multiple simulations, followed by the min. and max. values across them.",
                Color = new Disqord.Color(0x968867),
                Url = ammoItem.BallisticsLink
            };

            var penetrationResult = "";
            for (int i = 0; i < simulation.PenetrationChance.Length; i++)
                penetrationResult += $"@**{(1f + i) / simulation.PenetrationChance.Length:P}**" +
                    $" durability: **{simulation.PenetrationChance[i]:0.00;-#.00}**% chance\n";
            embed.AddField("Penetration Chances", string.Join("", penetrationResult));

            embed.AddField("Average Shots to 50 Damage:", $"**{simulation.AverageShotsTo50Damage.Mean:F2}** shots " +
                $"({simulation.AverageShotsTo50Damage.Min:F2}-{simulation.AverageShotsTo50Damage.Max:F2})", true);
            embed.AddField("Average Shots to Destruction:", $"**{simulation.AverageShotsToDestruction.Mean:F2}** shots " +
                $"({simulation.AverageShotsToDestruction.Min:F2}-{simulation.AverageShotsToDestruction.Max:F2})", true);

            var buckshotNotice = ammoItem.Type == "buckshot" ?
                " For these simulations, projectile dispersion was taken into account." : "";
            embed.AddField("Notice", $"Data was simulated given no weapon velocity modifiers or attachments and a {simulation.Distance}m zero. {buckshotNotice}");

            embed.WithFooter($"Last modified {simulation.Modified.Humanize()}");

            return Response(embed);
        }
    }
}
