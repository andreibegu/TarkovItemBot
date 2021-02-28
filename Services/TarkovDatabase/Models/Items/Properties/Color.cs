using DiscordColor = Discord.Color;
using StandardColor = System.Drawing.Color;

namespace TarkovItemBot.Services.TarkovDatabase
{
    public class Color
    {
        public int A { get; set; }
        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }

        public static implicit operator StandardColor(Color color)
            => StandardColor.FromArgb(color.A, color.R, color.G, color.B);

        public static implicit operator DiscordColor(Color color)
            => new DiscordColor(color.R, color.G, color.B);
    };
}
