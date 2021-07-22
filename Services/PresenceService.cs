using Disqord;
using Disqord.Bot.Hosting;
using Disqord.Gateway;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;
using TarkovItemBot.Options;

namespace TarkovItemBot.Services
{
    public class PresenceService : DiscordBotService
    {
        private readonly BotOptions _config;

        public PresenceService(IOptions<BotOptions> config)
        {
            _config = config.Value;
        }

        protected override async ValueTask OnReady(ReadyEventArgs args)
        {
            await Bot.SetPresenceAsync(
                new LocalActivity(_config.Prefix + "help", ActivityType.Playing));
        }
    }
}
