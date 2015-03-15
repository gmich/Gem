using System;
using Microsoft.Xna.Framework;

namespace Gem.Network.Shooter.Client.Camera
{
   public class Camera
    {
        #region Declarations

        private Vector2 viewPortSize;
        private Vector2 position;

        #endregion

        #region Constructor

        public Camera(Vector2 initialPos, Vector2 viewportSize)
        {
            this.Position = initialPos;
            this.viewPortSize = viewportSize;
        }

        #endregion

        #region Properties

        public Vector2 Position
        {
            get { return position; }
            set
            {
                position = new Vector2(MathHelper.Clamp(value.X, 0, WorldRectangle.Width),
                    MathHelper.Clamp(value.Y, 0, WorldRectangle.Height));
            }
        }

        public Rectangle WorldRectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, 1000 * 48, 50 * 48);
            }
        }

        public int ViewPortWidth
        {
            get { return (int)viewPortSize.X; }
            set
            {
                viewPortSize.X = value;
            }
        }

        public int ViewPortHeight
        {
            get { return (int)viewPortSize.Y; }
            set
            {
                viewPortSize.Y = value;
            }
        }
        public Vector2 WorldCenter
        {
            get
            {
                return Position + new Vector2(ViewPortWidth / 2, ViewPortHeight / 2);
            }
        }

        public Rectangle ScreenRectangle
        {
            get
            {
                return new Rectangle(0, 0,ViewPortWidth, ViewPortHeight);
            }
        }

        public Rectangle ViewPort
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, ViewPortWidth, ViewPortHeight);
            }
        }

        #endregion

        #region Public Methods
       
        public void Move(Vector2 offset)
        {
            Position += offset;
        }
       
        public bool inScreenBounds(Vector2 location)
        {
            return (location.X > Position.X && location.X < Position.X + ViewPortWidth
                 && location.Y > Position.Y && location.Y < Position.Y + ViewPortHeight);
        }

        public Vector2 AdjustInWorldBounds(Vector2 location, float width, float height)
        {
            location.X = MathHelper.Clamp(location.X, position.X, position.X + ViewPortWidth - width);
            location.Y = MathHelper.Clamp(location.Y, position.Y, position.Y + ViewPortHeight - height);
            return location;
        }

        public Vector2 AdjustInWorldBounds(Vector2 location, float width, float height,Vector2 origin)
        {
            location.X = MathHelper.Clamp(location.X, position.X+origin.X, position.X + ViewPortWidth - width+origin.X);
            location.Y = MathHelper.Clamp(location.Y, position.Y+origin.Y, position.Y + ViewPortHeight - height+origin.Y);
            return location;
        }

        public Vector2 AdjustInScreenBounds(Vector2 position,float dimension)
        {
            position.X = MathHelper.Clamp(position.X, dimension / 2, ViewPortWidth - dimension / 2);
            position.Y = MathHelper.Clamp(position.Y, dimension / 2, ViewPortHeight - dimension / 2);
            return position;
        }

        public bool ObjectIsVisible(Rectangle bounds)
        {
            return (ViewPort.Intersects(bounds));
        }

        public bool ObjectOnScreenBounds(Rectangle bounds)
        {
            return (ScreenRectangle.Intersects(bounds));
        }

        public Vector2 WorldToScreen(Vector2 worldLocation)
        {
            return worldLocation - position;
        }

        public Rectangle WorldToScreen(Rectangle worldRectangle)
        {
            return new Rectangle(worldRectangle.Left - (int)position.X, worldRectangle.Top - (int)position.Y, worldRectangle.Width, worldRectangle.Height);
        }
       
        public Vector2 VectorWorldToScreen(Vector2 worldLocation)
        {
            return new Vector2(worldLocation.X - Position.X, worldLocation.Y - Position.Y);
        }

        public Vector2 ScreenToWorld(Vector2 screenLocation)
        {
            return screenLocation + position;
        }

        public Rectangle ScreenToWorld(Rectangle screenRectangle)
        {
            return new Rectangle(screenRectangle.Left + (int)position.X, screenRectangle.Top + (int)position.Y, screenRectangle.Width, screenRectangle.Height);
        }

        #endregion

    }
}
