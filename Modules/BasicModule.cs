using Discord;
using Discord.Commands;
using Humanizer;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TarkovItemBot.Helpers;
using TarkovItemBot.Options;

namespace TarkovItemBot.Modules
{
    [Name("Basic")]
    public class BasicModule : ItemBotModuleBase
    {
        private readonly CommandService _commands;
        private readonly BotOptions _config;

        public BasicModule(IOptions<BotOptions> config, CommandService commands)
        {
            _config = config.Value;
            _commands = commands;
        }

        [Command("ping")]
        [Alias("latency", "pong")]
        [Summary("Returns the bot's latency to the Discord servers.")]
        [Remarks("ping")]
        public Task PingAsync()
            => ReplyAsync($"Current latency: `{Context.Client.Latency}`");

        [Command("about")]
        [Alias("info")]
        [Summary("Displays general information about the bot.")]
        [Remarks("about")]
        public async Task AboutAsync()
        {
            var builder = new EmbedBuilder()
            {
                Description = "A free and open source Discord bot providing item and location information for the game Escape from Tarkov.",
                Color = new Color(0x968867)
            };
            
            builder.WithAuthor($"{Context.Client.CurrentUser.Username} (v{AssemblyHelper.GetInformationalVersion()})",
                Context.Client.CurrentUser.GetAvatarUrl());

            builder.WithFooter($"(?) Use {_config.Prefix}help for command info");

            builder.AddField("Data Source", "[Tarkov Database](https://tarkov-database.com/)", true);
            builder.AddField("Source Code", "[Github](https://github.com/Andrewww1/TarkovItemBot)", true);

            var appInfo = await Context.Client.GetApplicationInfoAsync();

            builder.AddField("Instance Owner", appInfo.Owner.ToString(), true);

            if (appInfo.IsBotPublic) builder.AddField("Invite Link", $"[Invite](https://discord.com/oauth2/authorize?client_id={appInfo.Id}&scope=bot&permissions=16384)", true);

            builder.AddField("Guilds", Context.Client.Guilds.Count, true);

            var uptime = (DateTime.Now - Process.GetCurrentProcess().StartTime).Humanize();
            builder.AddField("Uptime", uptime, true);

            await ReplyAsync(embed: builder.Build());
        }

        [Command("help")]
        [Alias("h")]
        [Summary("Lists all commands available for use.")]
        [Remarks("help")]
        public async Task HelpAsync()
        {
            var builder = new EmbedBuilder()
            {
                Title = $"{Context.Client.CurrentUser.Username} Help",
                Color = new Color(0x968867),
                Description = $"A list of commands available for use. Prefix: `{_config.Prefix}`"
            }.WithFooter($"(?) Use {_config.Prefix}about for more info");

            foreach (var module in _commands.Modules.Where(x => x.Parent == null))
            {
                if (!module.Commands.Any()) continue;
                builder.AddField($"{module.Name} Commands", module.Commands.Humanize(x => $"`{x.GetUsage()}`"));
            }

            await ReplyAsync(embed: builder.Build());
        }

        [Command("help")]
        [Alias("h")]
        [Summary("Returns information about a specific command.")]
        [Remarks("help item")]
        public async Task HelpAsync([Remainder] string query)
        {
            var result = _commands.Search(query);

            if (!result.IsSuccess)
            {
                await ReplyAsync(result.ErrorReason);
                return;
            }

            var commandResult = result.Commands.FirstOrDefault();
            var command = commandResult.Command;

            var builder = new EmbedBuilder()
            {
                Title = $"{_config.Prefix}{commandResult.Alias}",
                Color = new Color(0x968867),
                Description = command.Summary ?? "No summary."
            };

            builder.AddField("Usage", $"`{command.GetUsage()}`", true);
            builder.AddField("Example", $"`{command.Remarks}`" ?? "None", true);
            builder.AddField("Aliases", command.Aliases.Humanize(x => $"`{x}`"), true);

            builder.WithFooter($"{command.Module.Name} Module • Prefix {_config.Prefix}");

            await ReplyAsync(embed: builder.Build());
        }
    }
}
