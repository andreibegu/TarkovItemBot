using ConsoleTables;
using Disqord;
using Disqord.Bot;
using Humanizer;
using Qmmands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TarkovItemBot.Helpers;
using TarkovItemBot.Services.TarkovDatabase;
using TarkovItemBot.Services.TarkovDatabaseSearch;

namespace TarkovItemBot.Modules
{
    [Name("Ballistics")]
    public class BallisticsModule : DiscordModuleBase
    {
        private readonly TarkovDatabaseClient _tarkov;
        private readonly TarkovSearchClient _tarkovSearch;

        public BallisticsModule(TarkovDatabaseClient tarkov, TarkovSearchClient tarkovSearch)
        {
            _tarkov = tarkov;
            _tarkovSearch = tarkovSearch;
        }

        [Command("rangecard", "card", "range", "ammocard")]
        [Description("Displays a range card for a specific cartridge.")]
        [Cooldown(5, 1, CooldownMeasure.Minutes, CooldownBucketType.User)]
        [Remarks("rangecard m995")]
        public async Task<DiscordCommandResult> BallisticsAsync(
            [Remainder][Range(3, 100, true, true)][Description("The item to display the range card for.")] string query)
        {
            var result = (await _tarkovSearch.SearchAsync($"name:{query}", DocType.Item, 1)).FirstOrDefault();

            if (result == null)
            {
                return Reply("No items found for query!");
            }

            var item = await _tarkov.GetItemAsync(result.Id, result.Kind);

            if (item is not AmmunitionItem ammoItem)
            {
                return Reply("The item provided is not a cartridge!");
            }

            var embed = new LocalEmbed()
            {
                Title = $"{item.Name} ({item.ShortName})",
                Color = item.Grid.Color,
                ThumbnailUrl = item.IconUrl,
                Description = item.Description,
                Url = ammoItem.BallisticsLink
            };

            embed.AddField("Initial Damage", ammoItem.Damage, true);
            embed.AddField("Initial Penetration", ammoItem.Penetration, true);
            embed.AddField("Initial Velocity", $"{ammoItem.Velocity}m/s", true);

            var interval = ammoItem.Type == "buckshot" ? 25 : 100;
            var gte = ammoItem.Type == "buckshot" ? 25 : 100;
            var lte = ammoItem.Type == "buckshot" ? 250 : 1000;

            var statsResult = await _tarkov.GetDistanceStatisticsAsync(item.Id, gte, lte);

            if (statsResult == null)
            {
                return Reply("No simulation results found for the given item!");
            }

            var stats = statsResult
                .Where(x => x.Distance % interval == 0);

            var card = new ConsoleTable("Range", "Velocity", "Damage", "Pen.", "Drop", "TOF");

            foreach (var stat in stats)
                card.AddRow($"{stat.Distance}m", $"{stat.Velocity:F2}m/s", $"{stat.Damage:F2}",
                    $"{stat.PenetrationPower:F2}", $"{stat.Drop:F2}cm", $"{stat.TimeOfFlight:F2}s");

            embed.AddField("Range Card", $"```{card.ToMinimalString()}```");

            embed.AddField("Notice", $"Range card was generated given no zeroing, weapon velocity modifiers or attachments.");

            var maxModified = stats.Max(x => x.Modified);
            embed.WithFooter($"Last modified {maxModified.Humanize()}");

            return Reply(embed);
        }

        [Command("armorsimulation", "armorsim", "armorimpact", "armori")]
        [Description("Displays armor simulation data for a specific cartridge and armor.")]
        [Cooldown(5, 1, CooldownMeasure.Minutes, CooldownBucketType.User)]
        [Remarks("armorsimulation m995 6b43 25")]
        public async Task<DiscordCommandResult> ArmorAsync(
            [Range(3, 100, true, true)][Description("The ammo to simulate against.")] string ammo,
            [Range(3, 100, true, true)][Description("The armor to simulate against.")] string armor,
            [Description("The range to simulate at.")] int range = 20)
        {
            var ammoResult = (await _tarkovSearch.SearchAsync($"name:{ammo}", DocType.Item, 1)).FirstOrDefault();
            var armorResult = (await _tarkovSearch.SearchAsync($"name:{armor}", DocType.Item, 1)).FirstOrDefault();

            if (ammoResult == null || armorResult == null)
            {
                return Reply("No items found for query!");
            }

            var ammoItem = await _tarkov.GetItemAsync(ammoResult.Id, ammoResult.Kind);

            if (ammoItem is not AmmunitionItem)
            {
                return Reply("The item provided is not a cartridge!");
            }

            var armorItem = await _tarkov.GetItemAsync(armorResult.Id, armorResult.Kind);

            if (armorItem is not ArmorItem)
            {
                return Reply("The item provided is not armor!");
            }

            var results = await _tarkov.GetArmorStatisticsAsync(ammoItem.Id, armorItem.Id, 0, 1000);

            if (results == null)
            {
                return Reply("No simulation results found for the given items!");
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
                Url = (ammoItem as AmmunitionItem).BallisticsLink
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

            var buckshotNotice = (ammoItem as AmmunitionItem).Type == "buckshot" ?
                " For these simulations, projectile dispersion was taken into account." : "";
            embed.AddField("Notice", $"Data was simulated given no weapon velocity modifiers or attachments and a {simulation.Distance}m zero. {buckshotNotice}");

            embed.WithFooter($"Last modified {simulation.Modified.Humanize()}");

            return Reply(embed);
        }
    }
}
