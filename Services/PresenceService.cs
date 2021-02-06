using Discord;
using Discord.Addons.Hosting;
using Discord.WebSocket;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;
using TarkovItemBot.Options;

namespace TarkovItemBot.Services
{
    public class PresenceService : InitializedService
    {
        private readonly DiscordSocketClient _discord;
        private readonly BotOptions _config;

        public PresenceService(DiscordSocketClient discord, IOptions<BotOptions> config)
        {
            _discord = discord;
            _config = config.Value;

            _discord.LatencyUpdated += LatencyUpdatedAsync;
        }

        private async Task LatencyUpdatedAsync(int before, int after)
        {
            if (_discord.ConnectionState != ConnectionState.Connected) return;
            var curentStatus = _discord.CurrentUser.Status;

            var newStatus = after switch
            {
                var i when i > 150 && i < 500 => UserStatus.AFK,
                var i when i >= 500 => UserStatus.DoNotDisturb,
                _ => UserStatus.Online
            };

            if (curentStatus != newStatus)
                await _discord.SetStatusAsync(newStatus);
        }

        public override Task InitializeAsync(CancellationToken cancellationToken)
            => _discord.SetGameAsync(_config.Prefix + "help");
    }
}
