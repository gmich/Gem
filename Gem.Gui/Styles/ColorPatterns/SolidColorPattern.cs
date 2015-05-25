using Microsoft.Xna.Framework;
using System;

namespace Gem.Gui.Styles
{
    internal class SolidColorPattern : IColorPattern
    {
        private readonly Color color;

        public SolidColorPattern(Color color)
        {
            this.color = color;
        }

        public Color[] Get(int sizeX, int sizeY)
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
