using DisqordColor = Disqord.Color;
using StandardColor = System.Drawing.Color;

namespace TarkovItemBot.Services.TarkovDatabase
{
    public class Color
    {
        public byte A { get; set; }
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }

        public static implicit operator StandardColor(Color color)
            => StandardColor.FromArgb(color.A, color.R, color.G, color.B);

        public static implicit operator DisqordColor(Color color)
            => new DisqordColor(color.R, color.G, color.B);
    };
}
