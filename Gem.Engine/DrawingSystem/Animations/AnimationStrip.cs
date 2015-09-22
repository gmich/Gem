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
        private readonly int tileSheetColumns;
        private readonly int tileSheetRows;
        private readonly int tileSheetCount;
        private int currentFrame;

        public EventHandler onAnimationFinished;
        public AnimationStripSettings Settings { get; }

        public AnimationStrip(AnimationStripSettings settings)
        {
            this.Settings = settings;
            currentFrame = settings.StartFrame - 1;
            tileSheetColumns = ((settings.TileSheetWidth - 1) / settings.FrameWidth) + 1;
            tileSheetRows = ((settings.TileSheetHeight - 1) / settings.FrameHeight) + 1;
            tileSheetCount = settings.LastFrame;

            frameUpdateTimer = new GTimer(settings.FrameDelay, settings.FrameDelay, NextFrame);
        }
        
        public Rectangle Frame { get; private set; }

        private void NextFrame(double timeDelta)
        {
            currentFrame = (currentFrame >= tileSheetCount) ?
                ResetTileSheet() : currentFrame + 1;

            int frameInRow = currentFrame / tileSheetColumns;
            int frameinColumn = currentFrame % tileSheetColumns;

            Frame = new Rectangle(
                frameinColumn * Settings.FrameWidth,
                frameInRow * Settings.FrameHeight,
                Settings.FrameWidth,
                Settings.FrameHeight);
        }

        private int ResetTileSheet()
        {
            onAnimationFinished?.Invoke(this, EventArgs.Empty);
            return Settings.StartFrame;
        }

        public void Update(double timeDelta)
        {
            frameUpdateTimer.Update(timeDelta);
        }
    }
}
