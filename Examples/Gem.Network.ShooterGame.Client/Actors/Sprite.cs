using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Gem.Network.Shooter.Client.Actors
{
    public class Sprite
    {
        #region Declarations

        public Texture2D Texture;
        private Vector2 location;
        private Vector2 velocity;
        protected List<Rectangle> frames;
        private int currentFrame;
        private float frameTime;
        private float timeForCurrentFrame;

        private Color tintColor = Color.White;

        public float rotation = 0.0f;
        public bool Expired = false;
        public bool Animate = true;
        public bool AnimateWhenStopped = true;

        public bool Collidable = true;
        public int CollisionRadius = 0;
        public int BoundingXPadding = 0;
        public int BoundingYPadding = 0;

        #endregion

        #region Constructor

        public Sprite(Vector2 worldLocation, Texture2D texture, Rectangle initialFrame, Vector2 velocity)
        {
            this.location = worldLocation;
            Texture = texture;
            this.velocity = velocity;
            worldLocation = Vector2.Zero;
            frames = new List<Rectangle>();
            frames.Add(initialFrame);
            frameTime = 0.1f;
            timeForCurrentFrame = 0.0f;
            this.Layer=0.0f;
            this.Scale = 1.0f;
            this.Transparency = 1.0f;
        }

        #endregion

        #region Properties

        public float Layer
        {
            get;
            set;
        }

        public float Scale
        {
            get;
            set;
        }

        public Camera.Camera2D Camera
        {
            get;
            set;
        }

        public int collisionRadius
        {
            set { CollisionRadius = value; }
        }

        #endregion

        #region Drawing and Animation Properties

        public int FrameWidth
        {
            get { return frames[0].Width; }
        }

        public int FrameHeight
        {
            get { return frames[0].Height; }
        }

        public Color TintColor
        {
            get { return tintColor; }
            set { tintColor = value; }
        }

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value % MathHelper.TwoPi; }
        }

        public int Frame
        {
            get { return currentFrame; }
            set
            {
                currentFrame = (int)MathHelper.Clamp(value, 0,
                    frames.Count - 1);
            }
        }

        public float FrameTime
        {
            get { return frameTime; }
            set { frameTime = MathHelper.Max(0, value); }
        }

        public Rectangle Source
        {
            get { return frames[currentFrame]; }
        }
        #endregion

        #region Positional Properties

        protected float Transparency
        {
            get;
            set;
        }

        public Vector2 WorldLocation
        {
            get { return location; }
            set { location = value; }
        }

        public Vector2 ScreenLocation
        {
            get
            {
                return Camera.WorldToScreen(location);
            }
        }

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        public Rectangle WorldRectangle
        {
            get
            {
                return new Rectangle(
                    (int)location.X,
                    (int)location.Y,
                    FrameWidth,
                    FrameHeight);
            }
        }

        public Rectangle ScreenRectangle
        {
            get
            {
                return Camera.WorldToScreen(WorldRectangle);
            }
        }

        public Vector2 RelativeCenter
        {
            get { return new Vector2(FrameWidth / 2, FrameHeight / 2); }
        }

        public Vector2 WorldCenter
        {
            get { return location + RelativeCenter; }
        }

        public Vector2 ScreenCenter
        {
            get
            {
                return Camera.WorldToScreen(location + RelativeCenter);
            }
        }
        #endregion

        #region Collision Related Properties

        public Rectangle BoundingBoxRect
        {
            get
            {
                return new Rectangle((int)location.X + BoundingXPadding, (int)location.Y + BoundingYPadding, FrameWidth - (BoundingXPadding * 2), FrameHeight - (BoundingYPadding * 2));
            }
        }

        #endregion

        #region Collision Detection Methods

        public bool IsBoxColliding(Rectangle OtherBox)
        {
            if ((Collidable) && (!Expired))
            {
                return BoundingBoxRect.Intersects(OtherBox);
            }
            else
            {
                return false;
            }
        }

        public bool IsCircleColliding( Vector2 otherCenter, float otherRadius)
        {
            if ((Collidable) && (!Expired))
            {
                if (Vector2.Distance(WorldCenter, otherCenter) <
                    (CollisionRadius + otherRadius))
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Animation-Related Methods

        public void AddFrame(Rectangle frameRectangle)
        {
            frames.Add(frameRectangle);
        }

        public void RotateTo(Vector2 direction)
        {
            Rotation = (float)Math.Atan2(direction.Y, direction.X);
        }

        #endregion

        #region Update and Draw

        public virtual void Update(GameTime gameTime)
        {
            if (!Expired)
            {
                float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

                timeForCurrentFrame += elapsed;

                if (Animate)
                {
                    if (timeForCurrentFrame >= FrameTime)
                    {
                        if ((AnimateWhenStopped) ||
                            (velocity != Vector2.Zero))
                        {
                            currentFrame = (currentFrame + 1) %
                                (frames.Count);
                            timeForCurrentFrame = 0.0f;
                        }
                    }
                }

                location += (velocity * elapsed);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (!Expired)
            {
                if (Camera.ObjectIsVisible(WorldRectangle))
                {
                    spriteBatch.Draw(Texture, ScreenCenter, Source, tintColor * Transparency, rotation, RelativeCenter, Scale, SpriteEffects.None, Layer);
                }
            }
        }
        #endregion
    }
}
