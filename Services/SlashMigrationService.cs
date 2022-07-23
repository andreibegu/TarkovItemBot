using Disqord;
using Disqord.Bot.Hosting;
using Disqord.Rest;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using TarkovItemBot.Options;

namespace TarkovItemBot.Services
{
    public class SlashMigrationService : DiscordBotService
    {
        private readonly BotOptions _config;

        public SlashMigrationService(IOptions<BotOptions> config)
        {
            _config = config.Value;
        }

        protected override async ValueTask OnMessageReceived(BotMessageReceivedEventArgs args)
        {
            if (!args.Message.Content.StartsWith(_config.Prefix))
                return;

            var msg = new LocalMessage()
                .WithContent("Moving forward, all comands will be handled using Discord's " +
                "Slash Command feature. As such, using the bot prefix is no longer supported. " +
                "To see a list of commands and interact with them, use `/` and select the bot's avatar. " +
                "Thank you for understanding!")
                .WithReply(args.MessageId);
            await args.Channel.SendMessageAsync(msg);
        }
    }
}
