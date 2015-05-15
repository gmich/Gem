using Microsoft.Xna.Framework;
using System;

namespace Gem.Gui.Animations
{
    public class AnimationContext
    {
        private readonly GameTime gameTime;

        public AnimationContext(GameTime gameTime)
        {
            this.gameTime = gameTime;
        }

        public double TotalMilliseconds
        {
            get { return gameTime.ElapsedGameTime.TotalMilliseconds; }
        }

        public double TotalSeconds
        {
            get { return gameTime.ElapsedGameTime.TotalSeconds; }
        }

        public double Time(Func<TimeSpan, double> timeCalculator)
        {
            return timeCalculator(this.gameTime.ElapsedGameTime);
        }
    }
}
