using Discord;
using Discord.Commands;
using Humanizer;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
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
        public Task PingAsync()
            => ReplyAsync($"Current latency: `{Context.Client.Latency}`");

        [Command("about")]
        [Alias("info")]
        public async Task AboutAsync()
        {
            var builder = new EmbedBuilder()
            {
                Description = "A free and open source Discord bot providing item and location information for the game Escape from Tarkov.",
                Color = new Color(0x968867)
            }.WithAuthor($"{Context.Client.CurrentUser.Username} ({Assembly.GetEntryAssembly().GetName().Version.ToString(3)})", Context.Client.CurrentUser.GetAvatarUrl())
            .WithFooter($"(?) Use {_config.Prefix}help for command info");

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
                builder.AddField($"{module.Name} Commands", module.Commands.Humanize(x => $"`{GetCommandUsage(x)}`"));
            }

            await ReplyAsync(embed: builder.Build());
        }

        private string GetCommandUsage(CommandInfo command)
        {
            var usage = command.Name;
            if (!command.Parameters.Any()) return usage;

            foreach (var parameter in command.Parameters)
            {
                if (parameter.IsOptional)
                    usage += $" [{parameter.Name} = {parameter.DefaultValue}]";
                else if (parameter.IsMultiple)
                    usage += $" |{parameter.Name}|";
                else if (parameter.IsRemainder)
                    usage += $" ...{parameter.Name}";
                else
                    usage += $" <{parameter.Name}>";
            }

            return usage;
        }
    }
}
