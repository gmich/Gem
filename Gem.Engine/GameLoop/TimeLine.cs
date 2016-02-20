using System;
using Microsoft.Xna.Framework;

namespace Gem.Engine.GameLoop
{
    public class Timeline : ITimeline
    {
        private readonly Func<double> timeProvider;

        public static ITimeline Default { get; } = new GameTimeline();

        public static ITimeline FromFactory(Func<double> timeProvider) => new Timeline(timeProvider);

        internal Timeline(Func<double> timeProvider)
        {
            this.timeProvider = timeProvider;
        }

        public TimeSpan DeltaTime
        {
            get { return TimeSpan.FromSeconds(timeProvider()); }
        }


    }
}

