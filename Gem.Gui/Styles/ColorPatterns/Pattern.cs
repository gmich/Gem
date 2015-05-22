using Microsoft.Xna.Framework;
using System;

namespace Gem.Gui.Styles
{
    public sealed class Pattern : IColorPattern
    {
        private readonly Func<int, int, Color[]> pattern;

        public Pattern(Func<int, int, Color[]> pattern)
        {
            this.pattern = pattern;
        }

        public Color[] Get(int sizeX, int sizeY)
        {
            return pattern(sizeX, sizeY);
        }

        public static IColorPattern SolidColor(Color color)
        {
            return new SolidColorPattern(color);
        }

        public static IColorPattern Border(Color border, Color filling)
        {
            return new BorderPattern(border,filling);
        }

        public static IColorPattern TextViewPattern(Color border, Color filling)
        {
            return new TextViewPattern(border, filling); 
        }

    }
}
