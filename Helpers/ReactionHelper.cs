using Discord;
using System.Collections.Generic;

namespace TarkovItemBot.Helpers
{
    public static class ReactionHelper
    {
        public static Dictionary<Emoji, int> EmojiNumberMap = new Dictionary<Emoji, int>
        {
            [new Emoji("\u0031\u20e3")] = 1,
            [new Emoji("\u0032\u20e3")] = 2,
            [new Emoji("\u0033\u20e3")] = 3,
            [new Emoji("\u0034\u20e3")] = 4,
            [new Emoji("\u0035\u20e3")] = 5,
        };
    }
}
