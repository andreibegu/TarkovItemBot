using Discord;
using Qmmands;
using System.Threading.Tasks;

namespace TarkovItemBot.Services.Commands
{
    public class DiscordModuleBase : ModuleBase<DiscordCommandContext>
    {
        protected Task<IUserMessage> ReplyAsync(string message = null, bool isTTS = false, Embed embed = null, RequestOptions options = null)
        {
            return base.Context.Message.ReplyAsync(message, isTTS, embed, AllowedMentions.None, options);
        }
    }
}
