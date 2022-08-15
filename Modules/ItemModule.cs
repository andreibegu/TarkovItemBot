using Disqord;
using Disqord.Bot;
using Disqord.Bot.Commands;
using Disqord.Bot.Commands.Application;
using Disqord.Extensions.Interactivity.Menus;
using Disqord.Rest;
using Humanizer;
using Microsoft.Extensions.Caching.Memory;
using Qmmands;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TarkovItemBot.Helpers;
using TarkovItemBot.Services.TarkovDatabase;
using TarkovItemBot.Services.TarkovDatabaseSearch;

namespace TarkovItemBot.Modules
{
    [Name("Item")]
    public class ItemModule : DiscordApplicationModuleBase
    {
        private readonly TarkovDatabaseClient _tarkov;
        private readonly TarkovSearchClient _tarkovSearch;
        private readonly IMemoryCache _cache;

        public ItemModule(TarkovDatabaseClient tarkov, TarkovSearchClient tarkovSearch,
            IMemoryCache cache)
        {
            _tarkov = tarkov;
            _tarkovSearch = tarkovSearch;
            _cache = cache;
        }

        [SlashCommand("total")]
        [Description("Returns the total items of a kind.")]
        [RateLimit(10, 1, RateLimitMeasure.Minutes, RateLimitBucketType.User)]
        public async Task<IResult> TotalAsync(
            [Description("The kind of the item group.")] string kind = "None")
        {
            var kindParsed = kind.DehumanizeTo<ItemKind>();
            var info = await _tarkov.GetItemIndexAsync();

            int total = kindParsed == ItemKind.None ? info.Total : info.Kinds[kindParsed].Count;
            var updated = kindParsed == ItemKind.None ? info.Modified : info.Kinds[kindParsed].Modified;

            return Response($"Total of items: `{total}` (Updated `{updated.Humanize()}`).");
        }

        [SlashCommand("item")]
        [Description("Returns detailed information for the item most closely matching the query.")]
        [RateLimit(10, 1, RateLimitMeasure.Minutes, RateLimitBucketType.User)]
        public async Task<IResult> ItemAsync(
            [Range(3, 100)][Description("The item to look for.")] string query)
        {
            IItem item;

            if (_cache.TryGetValue(query, out SearchItem result))
            {
                item = await _tarkov.GetItemAsync(result.Id, result.Kind);
            }
            else
            {
                result = (await _tarkovSearch.SearchAsync($"name:{query}", DocType.Item, 1))
                    .FirstOrDefault();

                if (result is null)
                    return Response("No item found for the given query!");

                item = await _tarkov.GetItemAsync(result.Id, result.Kind);
            }

            return Response(item.ToEmbed());
        }

        [AutoComplete("item")]
        [RateLimit(3, 1, RateLimitMeasure.Seconds, RateLimitBucketType.User)]
        public async Task ItemAutoComplete(AutoComplete<string> query)
        {
            if (!query.IsFocused || query.RawArgument.Length < 3)
                return;

            var results = (await _tarkovSearch.SearchAsync($"name:{query.RawArgument}", DocType.Item, 15));

            // Cache as workaround, choice being the only value being passed from autocomplete handler.
            foreach (var result in results)
            {
                _cache.Set(result.Name, result, TimeSpan.FromSeconds(30));
            }

            query.Choices.AddRange(results.Select(x => x.Name));
        }

        [AutoComplete("total")]
        public void TotalAutoComplete(AutoComplete<string> kind)
        {
            if (!kind.IsFocused)
                return;

            kind.Choices.AddRange(Enum.GetValues<ItemKind>().Select(x => x.Humanize())
                .Where(x => kind.RawArgument.Length == 0 
                    || x.Contains(kind.RawArgument, StringComparison.InvariantCultureIgnoreCase))
                .Take(25));
        }
    }
}
