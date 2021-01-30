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
        public async Task TotalAsync(ItemKind kind = ItemKind.None)
        {
            var info = await _tarkov.GetItemsInfoAsync();

            int total = kind == ItemKind.None ? info.Total : info.Kinds[kind].Count;

            await Context.Message.ReplyAsync($"Total of items: `{total}`.");
        }

    }
}
