using Microsoft.Xna.Framework;
using System;

namespace Gem.Gui.Styles
{
    public interface IPattern
    {
        Color[] Get(int sizeX, int sizeY, Color color);
    }

}
