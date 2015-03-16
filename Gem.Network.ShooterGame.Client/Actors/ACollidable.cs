using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Gem.Network.Shooter.Client.Actors
{
    using Level;
    using Camera;

    public abstract class ACollidable
    {
        #region Declarations

        protected Vector2 worldLocation;
        protected Vector2 velocity;
        protected Vector2 moveAmount;
        protected int frameWidth;
        protected int frameHeight;

        protected bool enabled;
        protected bool onGround;

        protected Rectangle collisionRectangle;
        protected int collideWidth;
        protected int collideHeight;

        protected float drawDepth = 0.85f;
        protected TileMap tileMap;
        protected CameraManager Camera;
        protected Texture2D texture;
        protected Color color;

        #endregion

        #region Properties
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        public Vector2 WorldLocation
        {
            get { return worldLocation; }
            set { worldLocation = value; }
        }

        public Vector2 MapLocation
        {
            get { return Camera.WorldToScreen(WorldLocation); }
        }

        public Vector2 Velocity
        {
            set { velocity = value; }
            get { return velocity; }
        }

        public Vector2 WorldCenter
        {
            get
            {
                return new Vector2(
                  (int)worldLocation.X + (int)(frameWidth / 2),
                  (int)worldLocation.Y + (int)(frameHeight / 2));
            }
        }

        public Rectangle WorldRectangle
        {
            get
            {
                return new Rectangle(
                    (int)worldLocation.X,
                    (int)worldLocation.Y,
                    frameWidth,
                    frameHeight);
            }
        }

        public Rectangle CollisionRectangle
        {
            get
            {
                return new Rectangle(
                    (int)worldLocation.X + collisionRectangle.X,
                    (int)WorldRectangle.Y + collisionRectangle.Y,
                    collisionRectangle.Width,
                    collisionRectangle.Height);
            }
            set { collisionRectangle = value; }
        }


        #endregion


        #region Map-Based Collision Detection Methods

        private Vector2 horizontalCollisionTest(Vector2 moveAmount)
        {
            if (moveAmount.X == 0)
                return moveAmount;

            Rectangle afterMoveRect = CollisionRectangle;
            afterMoveRect.Offset((int)moveAmount.X, 0);
            Vector2 corner1, corner2;

            if (moveAmount.X < 0)
            {
                corner1 = new Vector2(afterMoveRect.Left,
                                      afterMoveRect.Top + 1);
                corner2 = new Vector2(afterMoveRect.Left,
                                      afterMoveRect.Bottom - 1);
            }
            else
            {
                corner1 = new Vector2(afterMoveRect.Right,
                                      afterMoveRect.Top + 1);
                corner2 = new Vector2(afterMoveRect.Right,
                                      afterMoveRect.Bottom - 1);
            }

            Vector2 mapCell1 = tileMap.GetCellByPixel(corner1);
            Vector2 mapCell2 = tileMap.GetCellByPixel(corner2);

            if (!tileMap.CellIsPassable(mapCell1) || !tileMap.CellIsPassable(mapCell2))
            {
                moveAmount.X = 0;
                velocity.X = 0;
            }

            return moveAmount;
        }

        private Vector2 verticalCollisionTest(Vector2 moveAmount)
        {
            if (moveAmount.Y == 0)
                return moveAmount;

            Rectangle afterMoveRect = CollisionRectangle;
            afterMoveRect.Offset((int)moveAmount.X, (int)moveAmount.Y);
            Vector2 corner1, corner2;

            if (moveAmount.Y < 0)
            {
                corner1 = new Vector2(afterMoveRect.Left + 1,
                                      afterMoveRect.Top);
                corner2 = new Vector2(afterMoveRect.Right - 1,
                                      afterMoveRect.Top);
            }
            else
            {
                corner1 = new Vector2(afterMoveRect.Left + 1,
                                      afterMoveRect.Bottom);
                corner2 = new Vector2(afterMoveRect.Right - 1,
                                      afterMoveRect.Bottom);
            }

            Vector2 mapCell1 = tileMap.GetCellByPixel(corner1);
            Vector2 mapCell2 = tileMap.GetCellByPixel(corner2);

            if ((!tileMap.CellIsPassable(mapCell1) || !tileMap.CellIsPassable(mapCell2)))
            {
                if (moveAmount.Y > 0)
                {
                    onGround = true;
                }
                if ((mapCell1.Y - 1) * tileMap.TileHeight > worldLocation.Y)
                {
                    worldLocation.Y = (mapCell1.Y) * tileMap.TileHeight - frameHeight;
                }
                moveAmount.Y = 0;
                velocity.Y = 0;
            }

            return moveAmount;
        }

        #endregion

        #region Public Methods

        protected void CheckCollision(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (velocity.Y != 0)
            {
                onGround = false;
            }

            moveAmount = velocity * elapsed;

            moveAmount = horizontalCollisionTest(moveAmount);
            moveAmount = verticalCollisionTest(moveAmount);

            Vector2 newPosition = worldLocation + moveAmount;

            worldLocation = newPosition;
        }

        public abstract void Update(GameTime gameTime);

        public virtual void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(texture, Camera.WorldToScreen(WorldRectangle),
                new Rectangle(0, 0, frameWidth, frameHeight), color, 0.0f, Vector2.Zero, SpriteEffects.None, drawDepth);
        }
    }

        #endregion

}

