using Microsoft.Xna.Framework;
using System;

namespace Gem.Gui.Styles
{
    internal class BorderPattern : IPattern
    {
        public Color[] Get(int sizeX, int sizeY, Color color)
        {
            Color[] colorArray = new Color[sizeX * sizeY];
            
            for (int i = 0; i < sizeX * sizeY; i++)
            {
                colorArray[i] = color;
            }
            for (int i = 0; i < sizeX; i++)
            {
                colorArray[i] = Color.Black;
            }
            for (int i = 0; i < sizeX; i++)
            {
                colorArray[(sizeY - 1) * sizeX + i] = Color.Black;
            }
            for (int i = 0; i < sizeY; i++)
            {
                colorArray[i * sizeX] = Color.Black;
            }
            for (int i = 1; i < sizeY; i++)
            {
                colorArray[i * sizeX - 1] = Color.Black;
            }

            return colorArray;
        }
    }
}
