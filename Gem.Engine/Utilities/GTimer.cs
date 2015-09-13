using System;

namespace Gem.Utilities
{
    public class GTimer
    {
        private readonly double endTime;
        private readonly Action<double> timerEndCallback;
        private double elapsedTime;

        public GTimer(double initialTime, double endTime, Action<double> timerEndCallback)
        {
            elapsedTime = initialTime;
            this.endTime = endTime;
            this.timerEndCallback = timerEndCallback;
        }

        public void Update(double timeDelta)
        {
            elapsedTime += timeDelta;
            if(elapsedTime>=endTime)
            {
                timerEndCallback(timeDelta);
                elapsedTime = 0.0d;
            }
        }
    }
}
