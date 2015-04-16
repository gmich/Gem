using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Gem.Gui.Rendering
{

    public class Region
    {
        /// <summary>
        /// In case the origin is not the center of the element, provide a different calculation method
        /// </summary>
        private readonly Func<Region, Vector2> OriginCalculator;

        public Region(Vector2 position, Vector2 size, Func<Region, Vector2> originCalculator = null)
        {
            OriginCalculator = (originCalculator == null) ?
                                region => new Vector2(region.Size.X / 2, region.Size.Y / 2) :
                                originCalculator;
            this.position = position;
            this.size = size;
            this.origin = OriginCalculator(this);
            AdjustFrameBoundaries();
        }

        private Vector2 position;
        public Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
                AdjustFrameBoundaries();
            }
        }

        private Vector2 size;
        public Vector2 Size
        {
            get
            {
                return size;
            }
            set
            {
                size = value;
                AdjustFrameBoundaries();
                origin = OriginCalculator(this);
            }
        }

        private Vector2 origin;
        public Vector2 Origin
        {
            get
            {
                return origin;
            }
        }

        private Rectangle frame;
        public Rectangle Frame
        {
            get
            {
                return frame;
            }
        }

        /// <summary>
        /// This is invoked when the size or the position change
        /// </summary>
        private void AdjustFrameBoundaries()
        {
            frame = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
        }


    }


}
