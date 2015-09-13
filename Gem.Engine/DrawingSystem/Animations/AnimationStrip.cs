using Gem.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

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

        public AnimationStrip(int spriteSheetWidth, int spriteSheetHeight, AnimationStripSettings settings)
        {
            this.settings = settings;
            currentFrame = settings.StartFrame-1;
            tileSheetColumns = (spriteSheetWidth / settings.FrameWidth) + 1;
            tileSheetRows = (spriteSheetHeight / settings.FrameHeight) + 1;
            tileSheetCount = (settings.LastFrame == -1) ? 
                tileSheetColumns * tileSheetRows : settings.LastFrame;

            frameUpdateTimer = new GTimer(settings.FrameDelay, settings.FrameDelay, NextFrame);
        }

        public Rectangle Frame { get; private set; }

        private void NextFrame(double timeDelta)
        {
            currentFrame = (currentFrame == (tileSheetCount) - 1) ?
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
