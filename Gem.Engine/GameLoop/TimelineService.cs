using Microsoft.Xna.Framework;
using System;

namespace Gem.Engine.GameLoop
{
    public class TimelineService
    {
        private readonly GameTimeline timeLine = new GameTimeline();
        public ITimeline GameTimeline => timeLine;
        
        public ITimeline Create(Func<double> timeProvider)
        {
            return new TimeLine(timeProvider);
        }

        public void Update(GameTime gameTime)
        {
            timeLine.DeltaTime = gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
