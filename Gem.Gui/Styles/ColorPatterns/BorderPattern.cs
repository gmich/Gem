using Microsoft.Xna.Framework;
using System;

namespace Gem.Gui.Styles
{
    internal class BorderPattern : IColorPattern
    {
        private readonly Color filling;
        private readonly Color border;

        internal BorderPattern(Color border,Color filling)
        {
            this.border = border;
            this.filling = filling;
        }

        public Color[] Get(int sizeX, int sizeY)
        {
            Color[] colorArray = new Color[sizeX * sizeY];
            
            for (int i = 0; i < sizeX * sizeY; i++)
            {
                colorArray[i] = filling;
            }
            for (int i = 0; i < sizeX; i++)
            {
                colorArray[i] = border;
            }
            for (int i = 0; i < sizeX; i++)
            {
                colorArray[(sizeY - 1) * sizeX + i] = border;
            }
            for (int i = 0; i < sizeY; i++)
            {
                colorArray[i * sizeX] = border;
            }
            for (int i = 1; i < sizeY; i++)
            {
                colorArray[i * sizeX - 1] = border;
            }

            return colorArray;
        }
    }
}
