using Discord;
using Humanizer;
using Microsoft.Extensions.Options;
using Qmmands;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TarkovItemBot.Helpers;
using TarkovItemBot.Options;
using TarkovItemBot.Services.Commands;

namespace TarkovItemBot.Modules
{
    [Name("Basic")]
    public class BasicModule : DiscordModuleBase
    {
        private readonly CommandService _commands;
        private readonly BotOptions _config;

        public BasicModule(IOptions<BotOptions> config, CommandService commands)
        {
            _config = config.Value;
            _commands = commands;
        }

        [Command("ping", "latency", "pong")]
        [Description("Returns the bot's latency to the Discord servers.")]
        [Remarks("ping")]
        public Task PingAsync()
            => ReplyAsync($"Current latency: `{Context.Client.Latency}`");

        [Command("about", "info")]
        [Description("Displays general information about the bot.")]
        [Remarks("about")]
        public async Task AboutAsync()
        {
            var appInfo = await Context.Client.GetApplicationInfoAsync();

            var builder = new EmbedBuilder()
            {
                Description = appInfo.Description,
                Color = new Color(0x968867)
            };

            builder.WithAuthor($"{Context.Client.CurrentUser.Username} (v{AssemblyHelper.GetInformationalVersion()})",
                Context.Client.CurrentUser.GetAvatarUrl());

            builder.WithFooter($"(?) Use {_config.Prefix}help for command info");

            builder.AddField("Data Source", "[Tarkov Database](https://tarkov-database.com/) & " +
                "[Tarkov Tools](https://tarkov-tools.com/)", true);
            builder.AddField("Source Code", "[Github](https://github.com/Andrewww1/TarkovItemBot)", true);

            if (appInfo.IsBotPublic) builder.AddField("Invite Link", 
                $"[Invite](https://discord.com/oauth2/authorize?client_id={appInfo.Id}&scope=bot&permissions=16384)", true);

            builder.AddField("Instance Owner", appInfo.Owner.ToString(), true);
            builder.AddField("Guilds", Context.Client.Guilds.Count, true);

            var uptime = (DateTime.Now - Process.GetCurrentProcess().StartTime).Humanize();
            builder.AddField("Uptime", uptime, true);

            await ReplyAsync(embed: builder.Build());
        }

        [Command("help", "h")]
        [Description("Lists all commands available for use.")]
        [Remarks("help")]
        public async Task HelpAsync()
        {
            var builder = new EmbedBuilder()
            {
                Color = new Color(0x968867),
                Description = $"A list of commands available for use. Prefix commands with `{_config.Prefix}` or {Context.Client.CurrentUser.Mention}.\n" +
                    $"For more information on a specific command use `{_config.Prefix}help <command>`.\n"
            };

            builder.WithAuthor($"{Context.Client.CurrentUser.Username} Help", Context.Client.CurrentUser.GetAvatarUrl());
            builder.WithFooter($"(?) Use {_config.Prefix}about for more info");

            var modules = _commands.GetAllModules();
            foreach (var module in modules.Where(x => x.Parent == null)
                .OrderByDescending(x => x.Commands.Count))
            {
                if (!module.Commands.Any()) continue;
                builder.AddField($"{module.Name} Commands", string.Join("\n", module.Commands
                    .Select(x => $"• `{x.GetUsage()}`")), true);
            }

            builder.AddField("How to read parameter info", $"`<required>`, `[optional = Default]`, `...remainder`, `|multiple values|`.");

            await ReplyAsync(embed: builder.Build());
        }

        [Command("help", "h")]
        [Description("Returns information about a specific command.")]
        [Remarks("help item")]
        public async Task HelpAsync([Remainder] string query)
        {
            var result = _commands.FindCommands(query);

            if (!result.Any())
            {
                await ReplyAsync("No commands have been found!");
                return;
            }

            var commandResult = result[0];
            var command = commandResult.Command;

            string aliases = !command.Aliases.Any() ? "" : $"({string.Join(", ", command.Aliases)})";

            var builder = new EmbedBuilder()
            {
                Title = $"{_config.Prefix}{commandResult.Alias} {aliases}",
                Color = new Color(0x968867),
                Description = command.Description ?? "No command description."
            };

            builder.AddField("Usage", $"`{command.GetUsage()}`", true);
            builder.AddField("Example", $"`{command.Remarks}`" ?? "None", true);

            if (command.Parameters.Any())
            {
                var parameters = command.Parameters.Select(x => $"• `{x.Name}` - {x.Description}");
                builder.AddField("Parameters", string.Join("\n", parameters), false);
            }

            builder.WithFooter($"{command.Module.Name} Module • Prefix {_config.Prefix}");

            await ReplyAsync(embed: builder.Build());
        }
    }
}
