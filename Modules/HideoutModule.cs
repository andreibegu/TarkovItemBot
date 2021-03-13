using Discord;
using Humanizer;
using Qmmands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TarkovItemBot.Helpers;
using TarkovItemBot.Services.Commands;
using TarkovItemBot.Services.TarkovDatabase;
using TarkovItemBot.Services.TarkovDatabaseSearch;

namespace TarkovItemBot.Modules
{
    [Name("Hideout")]
    public class HideoutModule : DiscordModuleBase
    {
        private readonly TarkovDatabaseClient _tarkov;
        private readonly TarkovSearchClient _tarkovSearch;

        public HideoutModule(TarkovDatabaseClient tarkov, TarkovSearchClient tarkovSearch)
        {
            _tarkov = tarkov;
            _tarkovSearch = tarkovSearch;
        }

        [Command("module", "mod", "area")]
        [Description("Returns detailed information for the hideout module most closely matching the query.")]
        [Cooldown(5, 1, CooldownMeasure.Minutes, CooldownType.User)]
        [Remarks("module \"Intelligence Center\" 3")]
        public async Task ModuleAsync(
            [Range(3, 32, true, true)][Description("The module to look for.")] string query,
            [Description("The level of the module to look for")] int level = 1)
        {
            var searchResult = await _tarkov.GetModulesAsync(2, query);
            // Workaround for some ambiguities
            var module = searchResult.OrderBy(x => x.Name.Length - query.Length).FirstOrDefault();

            if (module == null)
            {
                await ReplyAsync("No module found for query!");
                return;
            }

            if (level - 1 > module.Stages.Count)
            {
                await ReplyAsync($"The specified module only has {module.Stages.Count} level(s)!");
                return;
            }

            var stage = module.Stages[level - 1];

            var builder = new EmbedBuilder()
            {
                Title = module.Name,
                Description = stage.Description,
                Color = new Discord.Color(0x968867)
            };

            builder.AddField("Requires Power", module.RequiresPower ? "Yes" : "No", true);
            builder.AddField("Levels", $"{module.Stages.Count} (Showing {level})", true);
            var time = stage.ConstructionTime.TotalSeconds == 0 ? "Instant" : stage.ConstructionTime.Humanize(3);
            builder.AddField("Building Time", time, true);
            builder.WithFooter($"Modified {module.Modified.Humanize()}");

            if (stage.Materials.Any())
            {
                var materialRequirements = stage.Materials.GroupBy(x => x.Kind);
                var materials = new Dictionary<IItem, int>();

                foreach (var requirement in materialRequirements)
                {
                    var items = await _tarkov.GetItemsAsync(requirement.Key, requirement.Select(x => x.Id));

                    foreach (var item in items)
                        materials.Add(item, requirement.FirstOrDefault(x => x.Id == item.Id).Count);
                }

                builder.AddField("Building Materials", materials.Humanize(x => $"{x.Value:N0}x {x.Key.Name}"), false);
            }

            if (stage.RequiredModules.Any())
            {
                var requiredModules = await _tarkov.GetModulesAsync(stage.RequiredModules.Select(x => x.Id));
                var modules = requiredModules
                    .ToDictionary(x => x, x => stage.RequiredModules.FirstOrDefault(y => y.Id == x.Id).Stage);

                builder.AddField("Required Modules", modules.Humanize(x => $"{x.Key.Name} L{x.Value + 1}"));
            }

            if (stage.Requirements.Any())
                builder.AddField("Other Requirements", stage.Requirements.Humanize(x => $"{x.Name} L{x.Level}"));

            var bonuses = "";

            var types = stage.Bonuses.Select(x => x.Type);
            if (stage.Bonuses.Any())
            {
                bonuses = bonuses.AddBonuses(module, level, stage.Bonuses);
            }

            var previousBonuses = module.Stages.Take(level - 1).SelectMany(x => x.Bonuses)
                .Where(x => !types.Contains(x.Type)).Distinct(new BonusTypeComparer());
            if (previousBonuses.Any())
            {
                bonuses += "**Previous Levels**\n";
                bonuses = bonuses.AddBonuses(module, level, previousBonuses, true);
            }

            if (!string.IsNullOrEmpty(bonuses)) builder.AddField("Bonuses", bonuses);

            await ReplyAsync(embed: builder.Build());
        }

        [Command("crafting", "crafts", "craft", "production")]
        [Description("Returns crafting information about the queried item.")]
        [Cooldown(5, 1, CooldownMeasure.Minutes, CooldownType.User)]
        [Remarks("crafting car battery")]
        public async Task CraftingAsync(
            [Range(3, 100, true, true)][Remainder][Description("The item to find crafting information for.")] string query)
        {
            var result = (await _tarkovSearch.SearchAsync($"name:{query}", DocType.Item, 1)).FirstOrDefault();

            if (result == null)
            {
                await ReplyAsync("No items found for query!");
                return;
            }

            var resultsIn = await _tarkov.GetProductionsAsync(outcome: result.Id);
            var usedIn = await _tarkov.GetProductionsAsync(material: result.Id);

            var productions = resultsIn.Union(usedIn);
            if (!productions.Any())
            {
                await ReplyAsync("No crafting schemes found for query!");
                return;
            }

            var builder = new EmbedBuilder()
            {
                Title = result.Name,
                Description = result.Description,
                ThumbnailUrl = result.IconUrl,
                Url = result.WikiUrl,
                Color = new Discord.Color(0x968867)
            };

            var groups = productions.SelectMany(x => x.Materials.Union(x.Outcome)).GroupBy(x => x.Kind);
            var items = new Dictionary<string, IItem>();

            foreach (var group in groups)
            {
                var groupResult = await _tarkov.GetItemsAsync(group.Key, group.Select(x => x.Id));
                foreach (var groupedItem in groupResult)
                    items.Add(groupedItem.Id, groupedItem);
            }

            var modules = (await _tarkov.GetModulesAsync(productions.Select(x => x.Module)))
                .ToDictionary(x => x.Id, x => x);

            var productionList = "";
            foreach (var production in productions)
            {
                string lambda(ItemReference x) => x.Id == result.Id ? $"**{x.Count:N0}x {items[x.Id].ShortName}**" :
                    $"{x.Count:N0}x {items[x.Id].ShortName}";

                var materials = production.Materials.Any() ? production.Materials.Humanize(lambda) : "Recurring production, module dependant";
                var outcomes = production.Outcome.Humanize(lambda);

                var moduleReference = production.RequiredModules.FirstOrDefault(x => x.Id == production.Module);
                var stage = moduleReference == null ? "" : $" {moduleReference.Stage + 1}";
                productionList += $"• {materials}\n➝ {outcomes} " +
                    $"({production.Duration.Humanize(2)} in {modules[production.Module].Name}{stage})\n\n";
            }

            builder.AddField("Crafts", productionList);
            await ReplyAsync(embed: builder.Build());
        }
    }
}
