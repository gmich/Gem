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

        public AnimationStrip(Func<string, Texture2D> textureLoader, AnimationStripSettings settings)
        {
            Texture = textureLoader(settings.RelativePathToTexture);

            tileSheetColumns = (Texture.Width / settings.FrameWidth);
            tileSheetRows = (Texture.Width / settings.FrameHeight);
            tileSheetCount = tileSheetColumns * tileSheetRows;

            frameUpdateTimer = new GTimer(settings.FrameDelay, settings.FrameDelay, NextFrame);
        }

        public Rectangle Frame { get; private set; }
        public Texture2D Texture { get; }

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
