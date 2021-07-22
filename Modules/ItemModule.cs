﻿using Disqord;
using Disqord.Bot;
using Disqord.Extensions.Interactivity.Menus;
using Disqord.Rest;
using Humanizer;
using Qmmands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TarkovItemBot.Helpers;
using TarkovItemBot.Services.TarkovDatabase;
using TarkovItemBot.Services.TarkovDatabaseSearch;

namespace TarkovItemBot.Modules
{
    [Name("Item")]
    public class ItemModule : DiscordModuleBase
    {
        private readonly TarkovDatabaseClient _tarkov;
        private readonly TarkovSearchClient _tarkovSearch;

        public ItemModule(TarkovDatabaseClient tarkov, TarkovSearchClient tarkovSearch)
        {
            _tarkov = tarkov;
            _tarkovSearch = tarkovSearch;
        }

        [Command("total", "t")]
        [Description("Returns the total items of a kind.")]
        [Cooldown(10, 1, CooldownMeasure.Minutes, CooldownBucketType.User)]
        [Remarks("total Ammunition")]
        public async Task<DiscordCommandResult> TotalAsync(
            [Description("The kind of the item group.")] ItemKind kind = ItemKind.None)
        {
            var info = await _tarkov.GetItemIndexAsync();

            int total = kind == ItemKind.None ? info.Total : info.Kinds[kind].Count;
            var updated = kind == ItemKind.None ? info.Modified : info.Kinds[kind].Modified;

            return Reply($"Total of items: `{total}` (Updated `{updated.Humanize()}`).");
        }

        [Command("item", "i", "it")]
        [Description("Returns detailed information for the item most closely matching the query.")]
        [Cooldown(10, 1, CooldownMeasure.Minutes, CooldownBucketType.User)]
        [Remarks("item Zagustin")]
        public async Task<DiscordCommandResult> ItemAsync(
            [Remainder][Range(3, 100, true, true)][Description("The item to look for.")] string query)
        {
            var results = (await _tarkovSearch.SearchAsync($"name:{query}", DocType.Item, 25));

            if (results.Count == 0)
            {
                return Reply("No items found for query!");
            }

            if (results.Count == 1)
            {
                var result = results.First();
                var item = await _tarkov.GetItemAsync(result.Id, result.Kind);
                return Reply(item.ToEmbed());
            }

            var searchView = new SearchView(results, _tarkov);

            return View(searchView);
        }
    }
}
