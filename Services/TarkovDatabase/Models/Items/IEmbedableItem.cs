using Discord;

namespace TarkovItemBot.Services.TarkovDatabase
{
    public interface IEmbedableItem
    {
        public EmbedBuilder ToEmbedBuilder();
    }
}
