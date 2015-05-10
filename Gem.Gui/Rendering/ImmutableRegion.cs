using System;
using Microsoft.Xna.Framework;

namespace Gem.Gui.Rendering
{
    public struct ImmutableRegion
    {
        private readonly Vector2 position;
        private readonly Vector2 size;
        private readonly Rectangle frame;
        private readonly Vector2 origin;

        public ImmutableRegion(Vector2 position, Vector2 size, Vector2 origin)
        {
            this.position = position;
            this.size = size;
            this.origin = origin;
            this.frame = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
        }

        public ImmutableRegion(Region region)
            : this(region.Position, region.Size, region.Center)
        { }

        public Vector2 Size { get { return size; } }
        public Vector2 Position { get { return position; } }
        public Rectangle Frame { get { return frame; } }
        public Vector2 Origin { get { return origin; } }
    }
}
