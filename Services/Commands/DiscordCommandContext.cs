using Discord.WebSocket;
using Qmmands;
using System;

namespace TarkovItemBot.Services.Commands
{
    public class DiscordCommandContext : CommandContext
    {
        public DiscordSocketClient Client { get; }
        public SocketGuild Guild { get; }
        public ISocketMessageChannel Channel { get; }
        public SocketUserMessage Message { get; }
        public SocketUser User { get; }
        public SocketGuildUser Member { get; }

        public DiscordCommandContext(DiscordSocketClient client, SocketUserMessage message, IServiceProvider services) : base(services)
        {
            this.Client = client;
            this.Message = message;
            this.Channel = message.Channel;
            this.Guild = (this.Channel as SocketGuildChannel)?.Guild;
            this.User = message.Author;
            this.Member = message.Author as SocketGuildUser;
        }
    }
}
