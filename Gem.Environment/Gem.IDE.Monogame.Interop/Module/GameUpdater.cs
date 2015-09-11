using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Gem.IDE.MonoGame.Interop.Module
{
    internal class GameUpdater
    {
        private readonly Action<GameTime> updateMethod;
        private readonly Action requestDraw;
        private readonly TimeSpan targetElapsedTime;
        private readonly TimeSpan maxElapsedTime;
        private readonly GameTime gameTime;
        private Stopwatch gameTimer;
        private TimeSpan accumulatedElapsedTime;
        private long previousTicks;
        private int updateFrameLag;

        public GameUpdater(Action<GameTime> updateMethod, Action requestDraw)
        {
            this.updateMethod = updateMethod;
            this.requestDraw = requestDraw;
            targetElapsedTime = TimeSpan.FromSeconds(1f / 60f);
            maxElapsedTime = TimeSpan.FromMilliseconds(500);
            gameTime = new GameTime();
        }

        public bool IsRunning { get; private set; }
        public bool Drawing { get; set; }

        public void Start()
        {
            IsRunning = true;
            gameTimer = Stopwatch.StartNew();

            Task.Factory.StartNew(RunLoop);
        }
        public void Stop()
        {
            IsRunning = false;
            gameTimer.Stop();
        }

        private void RunLoop()
        {
            while (IsRunning)
                Tick();
        }
        private void Tick()
        {
        RetryTick:

            if (!IsRunning)
                return;

            var currentTicks = gameTimer.Elapsed.Ticks;
            accumulatedElapsedTime += TimeSpan.FromTicks(currentTicks - previousTicks);
            previousTicks = currentTicks;

            if (accumulatedElapsedTime < targetElapsedTime)
            {
                var sleepTime = (int)(targetElapsedTime - accumulatedElapsedTime).TotalMilliseconds;

                Thread.Sleep(sleepTime);
                goto RetryTick;
            }

            if (accumulatedElapsedTime > maxElapsedTime)
                accumulatedElapsedTime = maxElapsedTime;

            gameTime.ElapsedGameTime = targetElapsedTime;
            var stepCount = 0;

            // Perform as many full fixed length time steps as we can.
            while (accumulatedElapsedTime >= targetElapsedTime)
            {
                if (!IsRunning)
                    return;

                gameTime.TotalGameTime += targetElapsedTime;
                accumulatedElapsedTime -= targetElapsedTime;
                ++stepCount;

                updateMethod(gameTime);
            }

            updateFrameLag += Math.Max(0, stepCount - 1);

            if (gameTime.IsRunningSlowly)
            {
                if (updateFrameLag == 0)
                    gameTime.IsRunningSlowly = false;
            }
            else if (updateFrameLag >= 5)
            {
                gameTime.IsRunningSlowly = true;
            }

            if (stepCount == 1 && updateFrameLag > 0)
                updateFrameLag--;

            gameTime.ElapsedGameTime = TimeSpan.FromTicks(targetElapsedTime.Ticks * stepCount);

            while (Drawing)
            {
                if (!IsRunning)
                    return;
            }
            Drawing = true;
            requestDraw();
        }
    }
}