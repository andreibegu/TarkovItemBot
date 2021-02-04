﻿using Discord;
using Discord.Commands;
using Humanizer;
using System.Linq;
using System.Threading.Tasks;

namespace TarkovItemBot.Modules
{
    public class BasicModule : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        public Task PingAsync()
            => Context.Message.ReplyAsync($"Current latency: `{Context.Client.Latency}`");
    }
}
