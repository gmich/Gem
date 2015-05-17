using Microsoft.Xna.Framework;
using System;

namespace Gem.Gui.Styles
{
    internal class SolidColorPattern : IPattern
    {
        public Color[] Get(int sizeX, int sizeY, Color color)
        {
            int totalSize = sizeX * sizeY;

            Color[] colorArray = new Color[totalSize];
            
            for (int arrayIndex = 0; arrayIndex < totalSize; arrayIndex++)
            {
                colorArray[arrayIndex] = color;
            }
            return colorArray;

        }
    }
}
