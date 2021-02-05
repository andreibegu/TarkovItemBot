using Discord;
using Discord.Commands;
using Discord.Net;
using Discord.WebSocket;
using Humanizer;
using Interactivity;
using Interactivity.Selection;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TarkovItemBot.Helpers;
using TarkovItemBot.Options;
using TarkovItemBot.Services;

namespace TarkovItemBot.Modules
{
    public class ItemModule : ModuleBase<SocketCommandContext>
    {
        private readonly TarkovDatabaseClient _tarkov;
        private readonly TarkovSearchClient _tarkovSearch;
        private readonly InteractivityService _interactive;
        private readonly BotOptions _config;

        public ItemModule(TarkovDatabaseClient tarkov, TarkovSearchClient tarkovSearch, InteractivityService interactive, IOptions<BotOptions> config)
        {
            _tarkov = tarkov;
            _tarkovSearch = tarkovSearch;
            _interactive = interactive;
            _config = config.Value;
        }

        [Command("total")]
        public async Task TotalAsync(ItemKind kind = ItemKind.None)
        {
            var info = await _tarkov.GetItemsInfoAsync();

            int total = kind == ItemKind.None ? info.Total : info.Kinds[kind].Count;
            var updated = kind == ItemKind.None ? info.Modified : info.Kinds[kind].Modified;

            await Context.Message.ReplyAsync($"Total of items: `{total}` (Updated `{updated.Humanize()}`).");
        }

        [Command("item")]
        [RequireBotPermission(ChannelPermission.ManageMessages | ChannelPermission.AddReactions)]
        public async Task ItemAsync([Remainder] string search)
        {
            if (search.Length < 3)
            {
                await Context.Message.ReplyAsync($"Query must be of 3 or more characters!");
                return;
            }

            var message = await Context.Message.ReplyAsync("Searching for items...");

            var searchResults = await _tarkovSearch.SearchAsync(search, 5);

            if (searchResults.Count == 0)
            {
                await message.ModifyAsync(x => x.Content = "No items found for query!");
                return;
            }

            if (searchResults.Count == 1 || !_config.EnableReactionChoice)
            {
                var result = searchResults.FirstOrDefault();
                var item = await _tarkov.GetEmbedableItemAsync(result.Id, result.Kind);
                await message.ModifyAsync(x => { x.Content = ""; x.Embed = item.ToEmbedBuilder().Build(); });
            }
            else
            {
                var choiceBuilder = new PageBuilder()
                {
                    Title = "Multiple results found!",
                    Description = string.Join("\n", searchResults.Select((elem, pos) => $"**{pos + 1}**. {elem.Name} (`{elem.Kind.Humanize()}`)"))
                }
                .WithFooter("Reply with the number associated to the item to select.");
                    
                var reactionMap = ReactionHelper.EmojiNumberMap.Take(searchResults.Count).ToDictionary(x => x.Key, x => x.Value);
                var reactionBuilder = new ReactionSelectionBuilder<int>()
                    .WithSelectables(reactionMap)
                    .WithUsers(Context.User)
                    .WithEnableDefaultSelectionDescription(false)
                    .WithSelectionEmbed(choiceBuilder)
                    .WithDeletion(DeletionOptions.Invalids);

                var choice = await _interactive.SendSelectionAsync(reactionBuilder.Build(), Context.Channel, TimeSpan.FromSeconds(30), message, runOnGateway: false);

                if (choice.IsSuccess)
                {
                    var index = choice.Value - 1;
                    var item = await _tarkov.GetEmbedableItemAsync(searchResults[index].Id, searchResults[index].Kind);
                    await message.ModifyAsync(x => { x.Content = ""; x.Embed = item.ToEmbedBuilder().Build(); });
                }
                else if (choice.IsCancelled)
                    await message.DeleteAsync();
            }
        }
    }
}
