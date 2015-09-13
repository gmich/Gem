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
        private int currentFrame = -1;

        public EventHandler onAnimationFinished;

        public AnimationStrip(int spriteSheetWidth, int spriteSheetHeight, AnimationStripSettings settings)
        {
            this.settings = settings;
            tileSheetColumns = (spriteSheetWidth / settings.FrameWidth);
            tileSheetRows = (spriteSheetHeight / settings.FrameHeight);
            tileSheetCount = tileSheetColumns * tileSheetRows;

            frameUpdateTimer = new GTimer(settings.FrameDelay, settings.FrameDelay, NextFrame);
        }

        public Rectangle Frame { get; private set; }

        private void NextFrame(double timeDelta)
        {
            currentFrame = (currentFrame == tileSheetColumns - 1) ?
                ResetTileSheet() : currentFrame + 1;

            int frameInRow = currentFrame / tileSheetRows;
            int frameinColumn = currentFrame % tileSheetRows;

            Frame = new Rectangle(
                frameinColumn * settings.FrameWidth,
                frameInRow * settings.FrameHeight,
                settings.FrameWidth,
                settings.FrameHeight);
        }

        private int ResetTileSheet()
        {
            onAnimationFinished?.Invoke(this, EventArgs.Empty);
            return 0;
        }
        
        public void Update(double timeDelta)
        {
            frameUpdateTimer.Update(timeDelta);
        }
    }
}
