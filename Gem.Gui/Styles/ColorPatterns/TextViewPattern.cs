using Microsoft.Xna.Framework;
using System;

namespace Gem.Gui.Styles
{
    internal class TextViewPattern : IColorPattern
    {
        private readonly Color border;
        private readonly Color filling;

        public TextViewPattern(Color border, Color filling)
        {
            this.filling = filling;
            this.border = border;
        }

        public Color[] Get(int sizeX, int sizeY)
        {
            Color[] colorArray = new Color[sizeX * sizeY];

            for (int i = 0; i < sizeX * sizeY; i++) colorArray[i] = filling;

            for (int i = 0; i < sizeX; i++) colorArray[i] = border;
            
            return colorArray;

        }
    }
}
