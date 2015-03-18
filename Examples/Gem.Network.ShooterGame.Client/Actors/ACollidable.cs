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

        protected Vector2 location;
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

        public bool Collided { get; set; }

        protected float Transparency { get; set; }
        #endregion

        #region Properties
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        public Vector2 WorldLocation
        {
            get { return location; }
            set { location = value; }
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
                  (int)location.X + (int)(frameWidth / 2),
                  (int)location.Y + (int)(frameHeight / 2));
            }
        }

        public Rectangle WorldRectangle
        {
            get
            {
                return new Rectangle(
                    (int)location.X,
                    (int)location.Y,
                    frameWidth,
                    frameHeight);
            }
        }

        public Rectangle CollisionRectangle
        {
            get
            {
                return new Rectangle(
                    (int)location.X + collisionRectangle.X,
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
                corner1 = new Vector2(location.X + moveAmount.X, location.Y);
                corner2 = new Vector2(location.X + moveAmount.X, location.Y + frameHeight-1);
            }
            else
            {
                corner1 = new Vector2(location.X + moveAmount.X + frameWidth, location.Y);
                corner2 = new Vector2(location.X + moveAmount.X + frameWidth, location.Y + frameHeight - 1);
            }

            Vector2 mapCell1 = tileMap.GetCellByPixel(corner1);
            Vector2 mapCell2 = tileMap.GetCellByPixel(corner2);

            if (!tileMap.CellIsPassable(mapCell1))
            {
                HorizontalCollision(mapCell1, ref moveAmount);
            }
            if (!tileMap.CellIsPassable(mapCell2))
            {
                HorizontalCollision(mapCell2, ref moveAmount);
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
                corner1 = new Vector2(location.X,location.Y+moveAmount.Y);
                corner2 = new Vector2(location.X + frameWidth, location.Y + moveAmount.Y);
            }
            else
            {
                corner1 = new Vector2(location.X, location.Y +collideHeight+ moveAmount.Y);
                corner2 = new Vector2(location.X + frameWidth, location.Y + frameHeight - 1 + moveAmount.Y);
            }

            Vector2 mapCell1 = tileMap.GetCellByPixel(corner1);
            Vector2 mapCell2 = tileMap.GetCellByPixel(corner2);

            if (!tileMap.CellIsPassable(mapCell1))
            {
                VerticalCollision(mapCell1, ref moveAmount);
            }
            if (!tileMap.CellIsPassable(mapCell2))
            {
                VerticalCollision(mapCell2, ref moveAmount);
            }

            return moveAmount;
        }

        private void VerticalCollision(Vector2 mapCell, ref Vector2 moveAmount)
        {
            if (moveAmount.Y > 0)
            {
                location = new Vector2(location.X, tileMap.GetCellLocation(mapCell).Y - this.frameHeight);
                onGround = true;
            }
            else if (moveAmount.Y < 0)
            {
                location = new Vector2(location.X, tileMap.GetCellLocation(mapCell).Y + tileMap.TileHeight);
            }

            moveAmount.Y = 0;
            velocity.Y = 0;
            Collided = true;
        }

        private void HorizontalCollision(Vector2 mapCell, ref Vector2 moveAmount)
        {
            if (moveAmount.X > 0)
                location = new Vector2(tileMap.GetCellLocation(mapCell).X - this.frameWidth - 1, location.Y);
            else if (moveAmount.X < 0)
                location = new Vector2(tileMap.GetCellLocation(mapCell).X + tileMap.TileWidth, location.Y);

            moveAmount.X = 0;
            velocity.X = 0;
            Collided = true;
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

            Vector2 newPosition = location + moveAmount;

            location = newPosition;
        }

        public abstract void Update(GameTime gameTime);

        public virtual void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(texture, Camera.WorldToScreen(WorldRectangle),
                new Rectangle(0, 0, frameWidth, frameHeight), color * Transparency, 0.0f, Vector2.Zero, SpriteEffects.None, drawDepth);
        }

        #endregion
    }

}

