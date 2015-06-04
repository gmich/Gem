using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Gem.Console.Rendering
{
    public class SpriteSheet : ITexture
    {

        #region Fields

        // The texture with animation frames
        Texture2D animationTexture;

        // The size and structure of whole frames sheet in animationTexture. The animationTexture could
        // hold animaton sequence organized in multiple rows and multiple columns, that's why animation 
        // engine should know how the frames are organized inside a frames sheet
        Point sheetSize;

        // Amount of time between frames
        TimeSpan frameInterval;

        // Time passed since last frame
        TimeSpan nextFrame;

        // Current frame in the animation sequence
        public Point currentFrame;

        public Point frameSize;

        #endregion

        #region Ctor 

        /// <summary>
        /// Constructor of an animation class
        /// </summary>
        /// <param name="frameSheet">Texture with animation frames sheet</param>
        /// <param name="size">Single frame size</param>
        /// <param name="frameSheetSize">The whole frame sheet size</param>
        /// <param name="interval">Interval between progressing to the next frame</param>
        public SpriteSheet(Texture2D frameSheet, Point frameSize, Point frameSheetSize, TimeSpan interval)
        {
            animationTexture = frameSheet;
            this.frameSize = frameSize;
            sheetSize = frameSheetSize;
            frameInterval = interval;
        }

        #endregion

        #region Update

        /// <summary>
        /// Updates the animaton progress
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="progressed">Returns true if animation were progressed; in such case 
        /// caller could updated the position of the animated character</param>
        public void Update(GameTime gameTime)
        {
            if (nextFrame >= frameInterval)
            {
                currentFrame.X++;
                if (currentFrame.X >= sheetSize.X)
                {
                    currentFrame.X = 0;
                    currentFrame.Y++;
                }
                if (currentFrame.Y >= sheetSize.Y)
                    currentFrame.Y = 0;
                nextFrame = TimeSpan.Zero;
            }
            else
            {
                nextFrame += gameTime.ElapsedGameTime;
            }
        }

        #endregion
        
        #region ITexture Members

        public Texture2D Texture
        {
            get { return animationTexture; }
        }

        public Rectangle Frame
        {
            get
            {
                return new Rectangle(
                        frameSize.X * currentFrame.X,
                        frameSize.Y * currentFrame.Y,
                        frameSize.X,
                        frameSize.Y);
            }
        }

        #endregion

    }
}