using Disqord;
using Disqord.Bot;
using Disqord.Bot.Commands;
using Disqord.Bot.Commands.Application;
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
    public class BasicModule : DiscordApplicationModuleBase
    {
        private readonly BotOptions _config;

        public BasicModule(IOptions<BotOptions> config)
        {
            _config = config.Value;
        }

        [SlashCommand("ping")]
        [Description("Returns the bot's latency to the Discord servers.")]
        public IResult Ping()
        {
            var time = DateTime.UtcNow - Context.Interaction.CreatedAt().UtcDateTime;
            return Response($"Current latency: `{time.Milliseconds}ms`");
        }

        [SlashCommand("about")]
        [Description("Displays general information about the bot.")]
        public async Task<IResult> AboutAsync()
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
            embed.AddField("Source Code", "[Github](https://github.com/andreibegu/TarkovItemBot)", true);

            if (appInfo.IsBotPublic) embed.AddField("Invite Link", 
                $"[Invite](https://discord.com/oauth2/authorize?client_id={appInfo.Id}&scope=bot&permissions=16384)", true);

            embed.AddField("Instance Owner", appInfo.Owner.ToString(), true);
            if (Context.Bot.GetGuilds().Count != 0) embed.AddField("Guilds", Context.Bot.GetGuilds().Count, true);

            var uptime = (DateTime.Now - Process.GetCurrentProcess().StartTime).Humanize();
            embed.AddField("Uptime", uptime, true);

            return Response(embed);
        }
    }
}
