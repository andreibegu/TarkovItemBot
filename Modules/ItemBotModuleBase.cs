using Discord;
using Discord.Commands;
using System.Threading.Tasks;

namespace TarkovItemBot.Modules
{
    public class ItemBotModuleBase : ModuleBase<SocketCommandContext>
    {
        protected override Task<IUserMessage> ReplyAsync(string message = null, bool isTTS = false, Embed embed = null, RequestOptions options = null, AllowedMentions allowedMentions = null, MessageReference messageReference = null)
        {
            return base.Context.Message.ReplyAsync(message, isTTS, embed, AllowedMentions.None, options);
        }
    }
}
