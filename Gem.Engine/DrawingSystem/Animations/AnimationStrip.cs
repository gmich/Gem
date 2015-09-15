using Gem.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Gem.DrawingSystem.Animations
{
    public class AnimationStrip
    {
        private readonly GTimer frameUpdateTimer;
        private readonly AnimationStripSettings settings;
        private readonly int tileSheetColumns;
        private readonly int tileSheetRows;
        private readonly int tileSheetCount;
        private int currentFrame;
        public EventHandler onAnimationFinished;
        private readonly List<Tuple<int, Rectangle>> frames = new List<Tuple<int, Rectangle>>();

        public AnimationStrip(int spriteSheetWidth, int spriteSheetHeight, AnimationStripSettings settings)
        {
            this.settings = settings;
            currentFrame = settings.StartFrame - 1;
            tileSheetColumns = (spriteSheetWidth / settings.FrameWidth) + 1;
            tileSheetRows = (spriteSheetHeight / settings.FrameHeight) + 1;
            tileSheetCount = settings.LastFrame;

            frameUpdateTimer = new GTimer(settings.FrameDelay, settings.FrameDelay, NextFrame);
        }

        public IEnumerable<Tuple<int, Rectangle>> ParseToEnd()
        {
            if (frames.Count == 0)
            {
                ParseSpriteSheet(settings.StartFrame);
            }
            return frames;
        }

        private void ParseSpriteSheet(int currentFrame)
        {

            int frameInRow = currentFrame / tileSheetColumns;
            int frameinColumn = currentFrame % tileSheetColumns;

            frames.Add(new Tuple<int, Rectangle>(
                currentFrame,
                    new Rectangle(
                    frameinColumn * settings.FrameWidth,
                    frameInRow * settings.FrameHeight,
                    settings.FrameWidth,
                    settings.FrameHeight)));

            int nextFrame = currentFrame + 1;
            if (nextFrame > tileSheetCount)
            {
                return;
            }
            ParseSpriteSheet(nextFrame);
        }


        public Rectangle Frame { get; private set; }

        private void NextFrame(double timeDelta)
        {
            currentFrame = (currentFrame >= tileSheetCount) ?
                ResetTileSheet() : currentFrame + 1;

            int frameInRow = currentFrame / tileSheetColumns;
            int frameinColumn = currentFrame % tileSheetColumns;

            Frame = new Rectangle(
                frameinColumn * settings.FrameWidth,
                frameInRow * settings.FrameHeight,
                settings.FrameWidth,
                settings.FrameHeight);
        }

        private int ResetTileSheet()
        {
            onAnimationFinished?.Invoke(this, EventArgs.Empty);
            return settings.StartFrame;
        }

        public void Update(double timeDelta)
        {
            frameUpdateTimer.Update(timeDelta);
        }
    }
}
