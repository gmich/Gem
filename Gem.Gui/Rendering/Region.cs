using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Gem.Gui.Rendering
{

    public class Region : IEquatable<Region>
    {

        #region EventArgs

        public class RegionEventArgs : EventArgs
        {
            public ImmutableRegion PreviousState { get; set; }
        }

        #endregion

        #region Fields

        /// <summary>
        /// In case the origin is not the center of the element, provide a different calculation method
        /// </summary>
        private readonly Func<Region, Vector2> CenterCalculator;

        /// <summary>
        /// Raised when the region changes position.
        /// The event argumentss contain the new region as the sender and the old state as an immutable region.
        /// </summary>
        public event EventHandler<RegionEventArgs> onPositionChange;

        /// <summary>
        /// Raised when the region changes size. 
        /// The event argumentss contain the new region as the sender and the old state as an immutable region.
        /// </summary>
        public event EventHandler<RegionEventArgs> onSizeChange;

        private Vector2 position;
        private Vector2 size;
        private Vector2 center;
        private Rectangle frame;

        private Vector2 virtualPosition;
        private Vector2 virtualSize;
        #endregion

        #region Ctor

        public Region(Vector2 position, Vector2 size, Func<Region, Vector2> centerCalculator = null)
        {
            CenterCalculator = (centerCalculator == null) ?
                                region => new Vector2(region.Size.X / 2, region.Size.Y / 2) :
                                centerCalculator;
            this.position = position;
            this.size = size;
            this.virtualSize = size;
            this.virtualPosition = position;

            this.center = CenterCalculator(this);
            AdjustFrameBoundaries();
        }

        public Region(float positionX, float positionY, float sizeX, float sizeY, Func<Region, Vector2> centerCalculator = null)
            : this(new Vector2(positionX, positionY), new Vector2(sizeX, sizeY), centerCalculator)
        { }

        #endregion

        #region Public Properties

        public static Region Empty
        {
            get
            {
                return new Region(Vector2.Zero, Vector2.Zero);
            }
        }

        public Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                var previousState = new ImmutableRegion(this);
                position = value;
                AdjustFrameBoundaries();

                OnPositionChange(new RegionEventArgs { PreviousState = previousState });
            }
        }

        public Vector2 VirtualSize
        {
            get
            {
                return virtualSize;
            }
            set
            {
                this.virtualSize = value;
                this.size = value;
            }
        }

        public Vector2 Size
        {
            get
            {
                return size;
            }
            set
            {
                var previousState = new ImmutableRegion(this);
                size = value;
                AdjustFrameBoundaries();
                center = CenterCalculator(this);

                OnSizeChange(new RegionEventArgs { PreviousState = previousState });
            }
        }

        public Vector2 Center
        {
            get
            {
                return center;
            }
        }

        public Rectangle Frame
        {
            get
            {
                return frame;
            }
        }

        public void Scale(Vector2 scale)
        {
            size = virtualSize * scale;
            AdjustFrameBoundaries();
            center = CenterCalculator(this);
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// This is invoked when the size or the position change
        /// </summary>
        private void AdjustFrameBoundaries()
        {
            frame = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
        }

        private void OnSizeChange(RegionEventArgs previousState)
        {
            var handler = onSizeChange;
            if (handler != null)
            {
                handler(this, previousState);
            }
        }

        private void OnPositionChange(RegionEventArgs previousState)
        {
            var handler = onPositionChange;
            if (handler != null)
            {
                handler(this, previousState);
            }
        }

        #endregion

        #region Equality Comparison

        public override bool Equals(object right)
        {
            if (Object.ReferenceEquals(right, null))
            {
                return false;
            }
            if (Object.ReferenceEquals(this, right))
            {
                return true;
            }
            if (this.GetType() != right.GetType())
            {
                return false;
            }
            return this.Equals(right as Region);
        }

        public bool Equals(Region other)
        {
            return ((this.position == other.position)
                   && (this.size == other.size));
        }

        public override int GetHashCode()
        {
            return position.GetHashCode()
                 ^ size.GetHashCode()
                 ^ center.GetHashCode();
        }

        #endregion

        #region Operator Overloading

        public static bool operator ==(Region left, Region right)
        {
            return left.Equals(right);
        }

        public static Region operator +(Region left, Region right)
        {
            return new Region(left.Position + right.Position, left.Size + right.Size);
        }

        public static bool operator !=(Region left, Region right)
        {
            return ((left.position != right.position)
                   || (left.size != right.size));
        }

        #endregion
    }

}
