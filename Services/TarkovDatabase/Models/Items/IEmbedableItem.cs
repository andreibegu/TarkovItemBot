using Discord;

namespace TarkovItemBot.Services
{
    public interface IEmbedableItem
    {
        public EmbedBuilder ToEmbedBuilder();
    }
}
