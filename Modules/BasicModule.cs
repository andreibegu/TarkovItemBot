using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using TarkovItemBot.Services;

namespace TarkovItemBot.Modules
{
    public class BasicModule : ModuleBase<SocketCommandContext>
    {
        private readonly TarkovDatabaseClient _tarkov;
        
        public BasicModule(TarkovDatabaseClient tarkov)
        {
            _tarkov = tarkov;
        }

        [Command("ping")]
        public Task PingAsync()
            => Context.Message.ReplyAsync($"Current latency: `{Context.Client.Latency}`");

        [Command("total")]
        public async Task TotalAsync()
        {
            var info = await _tarkov.GetItemsInfoAsync();
            await Context.Message.ReplyAsync($"Total number of items: `{info.Total}`");
        }

    }
}
