using System;


namespace Gem.Engine.GameLoop
{
    public class TimeLine : ITimeline
    {
        private readonly Func<double> timeProvider;

        public TimeLine(Func<double> timeProvider)
        {
            this.timeProvider = timeProvider;
        }

        public double DeltaTime
        {
            get { return timeProvider(); }
        }
    }
}

