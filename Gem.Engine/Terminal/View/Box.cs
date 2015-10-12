using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NullGuard;
using System;

namespace Gem.Engine.GTerminal.View
{
    [NullGuard(ValidationFlags.AllPublicArguments)]
    public class Box
    {
        public Box(Func<Rectangle> innerBounds,Color color)
        {
            InnerBounds = innerBounds;
            Color = color;
        }

        public Rectangle Bounds
        {
            get
            {
                var rect = InnerBounds();
                return new Rectangle(
                    rect.Left - Left,
                    rect.Right + Right,
                    rect.Top - Top,
                    rect.Bottom + Bottom);
            }
        }

        public Func<Rectangle> InnerBounds { get; set; }

        public int Top { get; set; }

        public int Bottom { get; set; }

        public int Left { get; set; }

        public int Right { get; set; }

        public Color Color { get; set; }

    }
}
