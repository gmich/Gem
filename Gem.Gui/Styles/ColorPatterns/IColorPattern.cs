using Microsoft.Xna.Framework;
using System;

namespace Gem.Gui.Styles
{
    public interface IColorPattern
    {
        Color[] Get(int sizeX, int sizeY);
    }

}
