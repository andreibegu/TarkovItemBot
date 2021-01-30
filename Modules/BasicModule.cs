using Discord.Commands;
using System.Threading.Tasks;

namespace TarkovItemBot.Modules
{
    public class BasicModule : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        public Task PingAsync()
            => ReplyAsync($"Current latency: `{Context.Client.Latency}`");
    }
}
