using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using TarkovItemBot.Helpers;
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

        [Command("item")]
        public async Task ItemAsync(string id)
        {
            // TODO: search and decide kind auto
            var item = await _tarkov.GetItemAsync<CommonItem>(id);

            var embed = new EmbedBuilder()
            {
                Title = $"{item.Name} ({item.ShortName})",
                Description = item.Description,
                ThumbnailUrl = item.IconUrl,
                Color = item.Grid.Color
            };

            // TODO: Add fields automatically (maybe attribute?)
            embed.AddField("Weight", $"{item.Weight} kg", true);
            embed.AddField("Rarity", item.Rarity.FirstCharUpper(), true);

            var width = item.Grid.Width;
            var height = item.Grid.Height;
            embed.AddField("Size", $"{width}x{height} ({width * height})", true);

            embed.AddField("Base Price", $"{item.Price} ₽");

            await Context.Message.ReplyAsync(embed: embed.Build());
        }
    }
}
