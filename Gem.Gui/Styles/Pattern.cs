using Microsoft.Xna.Framework;
using System;

namespace Gem.Gui.Styles
{
    public sealed class Pattern : IPattern
    {
        private readonly Func<int, int, Color, Color[]> pattern;

        public Pattern(Func<int, int, Color, Color[]> pattern)
        {
            this.pattern = pattern;
        }

        public Color[] Get(int sizeX, int sizeY, Color color)
        {
            return pattern(sizeX, sizeY, color);
        }

        private static Lazy<SolidColorPattern> _solidColor = new Lazy<SolidColorPattern>();
        public static IPattern SolidColor
        {
            get { return _solidColor.Value; }
        }

        private static Lazy<BorderPattern> _border = new Lazy<BorderPattern>();
        public static IPattern Border
        {
            get { return _border.Value; }
        }

    }
}
