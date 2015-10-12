using Microsoft.Xna.Framework;
using NullGuard;

namespace Gem.Engine.GTerminal.View
{
    [NullGuard(ValidationFlags.AllPublicArguments)]
    public class Box
    {
        public static Box Empty { get { return new Box(); } }

        public Rectangle Bounds(Rectangle outer) =>
                 new Rectangle(
                    outer.Left - Left,
                    outer.Right + Right,
                    outer.Top - Top,
                    outer.Bottom + Bottom);



        public int Top { get; set; }

        public int Bottom { get; set; }

        public int Left { get; set; }

        public int Right { get; set; }

        public Color Color { get; set; } = Color.White;

    }
}
