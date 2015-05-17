using Microsoft.Xna.Framework;
using System;

namespace Gem.Gui.Factories
{
    internal interface IPattern
    {
        Color[] Get(int sizeX, int sizeY, Color color);
    }

    internal static class Pattern
    {
        private static Lazy<SolidColorPattern> _solizeColor = new Lazy<SolidColorPattern>();
        internal static IPattern SolidColor
        {
            get { return _solizeColor.Value; }
        }
    }
}
