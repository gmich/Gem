using Microsoft.Xna.Framework;
using System;

namespace Gem.Gui.Styles
{
    internal class BorderPattern : IPattern
    {
        public Color[] Get(int sizeX, int sizeY, Color color)
        {
            throw new NotImplementedException();

            int totalSize = sizeX * sizeY;

            Color[] colorArray = new Color[totalSize];

            for (int arrayIndex = 0; arrayIndex < totalSize; arrayIndex++)
            {
                colorArray[arrayIndex] = (arrayIndex <= sizeX) ? Color.Red : color;
            }
            return colorArray;

        }
    }
}
