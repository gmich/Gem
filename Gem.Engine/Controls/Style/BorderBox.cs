using System;
using Microsoft.Xna.Framework;

namespace Gem.Engine.GTerminal.View
{
    public class BorderBox : Box
    {
        public static BorderBox Thin
        {
            get
            {
                return Create(1);
            }
        }

        public static BorderBox Create(int thickness)
        {
            return new BorderBox
            {
                Top = thickness,
                Bottom = thickness,
                Left = thickness,
                Right = thickness
            };
        }

        public IBorderStyle Style { get; set; } = BorderStyle.Solid;

    }
}
