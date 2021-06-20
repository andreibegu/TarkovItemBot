using Discord;
using Discord.Addons.Hosting;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;
using TarkovItemBot.Options;

namespace TarkovItemBot.Services
{
    public class PresenceService : DiscordClientService
    {
        private readonly DiscordSocketClient _client;
        private readonly BotOptions _config;

        public PresenceService(DiscordSocketClient client, IOptions<BotOptions> config, ILogger<PresenceService> log) : base(client, log)
        {
            _client = client;
            _config = config.Value;

            _client.LatencyUpdated += LatencyUpdatedAsync;
        }

        private async Task LatencyUpdatedAsync(int before, int after)
        {
            if (_client.ConnectionState != ConnectionState.Connected) return;
            var curentStatus = _client.CurrentUser.Status;

            var newStatus = after switch
            {
                var i when i > 150 && i < 500 => UserStatus.AFK,
                var i when i >= 500 => UserStatus.DoNotDisturb,
                _ => UserStatus.Online
            };

            if (curentStatus != newStatus)
                await _client.SetStatusAsync(newStatus);
        }

        protected override Task ExecuteAsync(CancellationToken cancellationToken)
            => _client.SetGameAsync(_config.Prefix + "help");
    }
}
