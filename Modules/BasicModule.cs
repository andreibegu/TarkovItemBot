using Disqord;
using Disqord.Bot;
using Disqord.Gateway;
using Disqord.Rest;
using Humanizer;
using Microsoft.Extensions.Options;
using Qmmands;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TarkovItemBot.Helpers;
using TarkovItemBot.Options;

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
        public DiscordCommandResult Ping()
        {
            var time = DateTime.UtcNow - Context.Message.CreatedAt().UtcDateTime;
            return Reply($"Current latency: `{time.Milliseconds}ms`");
        }

        [Command("about", "info")]
        [Description("Displays general information about the bot.")]
        [Remarks("about")]
        public async Task<DiscordCommandResult> AboutAsync()
        {
            var appInfo = await Context.Bot.FetchCurrentApplicationAsync();

            var embed = new LocalEmbed()
            {
                Description = appInfo.Description,
                Color = new Color(0x968867)
            };

            embed.WithAuthor($"{Context.Bot.CurrentUser.Name} (v{AssemblyHelper.GetInformationalVersion()})",
                Context.Bot.CurrentUser.GetAvatarUrl());

            embed.WithFooter($"(?) Use {_config.Prefix}help for command info");

            embed.AddField("Data Source", "[Tarkov Database](https://tarkov-database.com/) & " +
                "[Tarkov.dev](https://tarkov.dev/)", true);
            embed.AddField("Source Code", "[Github](https://github.com/Andrewww1/TarkovItemBot)", true);

            if (appInfo.IsBotPublic) embed.AddField("Invite Link", 
                $"[Invite](https://discord.com/oauth2/authorize?client_id={appInfo.Id}&scope=bot&permissions=16384)", true);

            embed.AddField("Instance Owner", appInfo.Owner.ToString(), true);
            embed.AddField("Guilds", Context.Bot.GetGuilds().Count, true);

            var uptime = (DateTime.Now - Process.GetCurrentProcess().StartTime).Humanize();
            embed.AddField("Uptime", uptime, true);

            return Reply(embed);
        }

        [Command("help", "h")]
        [Description("Lists all commands available for use.")]
        [Remarks("help")]
        public DiscordCommandResult Help()
        {
            var embed = new LocalEmbed()
            {
                Color = new Color(0x968867),
                Description = $"A list of commands available for use. Prefix commands with `{_config.Prefix}` or {Context.Bot.CurrentUser.Mention}.\n" +
                    $"For more information on a specific command use `{_config.Prefix}help <command>`.\n"
            };

            embed.WithAuthor($"{Context.Bot.CurrentUser.Name} Help", Context.Bot.CurrentUser.GetAvatarUrl());
            embed.WithFooter($"(?) Use {_config.Prefix}about for more info");

            var modules = _commands.GetAllModules();
            foreach (var module in modules.Where(x => x.Parent == null)
                .OrderByDescending(x => x.Commands.Count))
            {
                if (!module.Commands.Any()) continue;
                embed.AddField($"{module.Name} Commands", string.Join("\n", module.Commands
                    .Select(x => $"• `{x.GetUsage()}`")), true);
            }

            embed.AddField("How to read parameter info", $"`<required>`, `[optional = Default]`, `...remainder`, `|multiple values|`.");

            return Reply(embed);
        }

        [Command("help", "h")]
        [Description("Returns information about a specific command.")]
        [Remarks("help item")]
        public DiscordCommandResult Help([Remainder] string query)
        {
            var result = _commands.FindCommands(query);

            if (!result.Any())
            {
                return Reply("No commands have been found!");
            }

            var commandResult = result[0];
            var command = commandResult.Command;

            string aliases = !command.Aliases.Any() ? "" : $"({string.Join(", ", command.Aliases)})";

            var embed = new LocalEmbed()
            {
                Title = $"{_config.Prefix}{commandResult.Alias} {aliases}",
                Color = new Color(0x968867),
                Description = command.Description ?? "No command description."
            };

            embed.AddField("Usage", $"`{command.GetUsage()}`", true);
            embed.AddField("Example", $"`{command.Remarks}`" ?? "None", true);

            if (command.Parameters.Any())
            {
                var parameters = command.Parameters.Select(x => $"• `{x.Name}` - {x.Description}");
                embed.AddField("Parameters", string.Join("\n", parameters), false);
            }

            embed.WithFooter($"{command.Module.Name} Module • Prefix {_config.Prefix}");

            return Reply(embed);
        }
    }
}
