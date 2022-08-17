using Disqord;
using Disqord.Bot.Hosting;
using Disqord.Gateway;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;
using TarkovItemBot.Options;
using TarkovItemBot.Services.TarkovDatabase;

namespace TarkovItemBot.Services
{
    public class PresenceService : DiscordBotService
    {
        private readonly TarkovDatabaseClient _tarkov;

        public PresenceService(TarkovDatabaseClient tarkov)
        {
            _tarkov = tarkov;
        }

        protected override async ValueTask OnReady(ReadyEventArgs args)
        {
            var total = await _tarkov.GetItemIndexAsync();

            await Bot.SetPresenceAsync(
                new LocalActivity($"{total.Total} Tarkov items", ActivityType.Watching));
        }
    }
}
