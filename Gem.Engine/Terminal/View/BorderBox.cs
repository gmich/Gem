using System;
using Microsoft.Xna.Framework;

namespace Gem.Engine.GTerminal.View
{
    public class BorderBox : Box
    {
        public BorderBox(Func<Rectangle> bounds, Color color) 
            : base(bounds, color)
        {

        }

        public IBorderStyle Style { get; set; }

    }
}
