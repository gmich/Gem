using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Gem.Infrastructure
{
    /// <summary>
    /// Starts a task that runs in a separate thread
    /// </summary>
    public sealed class ParallelTaskStarter : IDisposable
    {

        #region Fields

        /// <summary>
        /// The token to stop the parallel task
        /// </summary>
        private CancellationTokenSource wtoken;

        /// <summary>
        /// The task
        /// </summary>
        private ActionBlock<DateTimeOffset> task;
    
        /// <summary>
        /// The task delay. 
        /// When a task run is finished, the thread pauses for as long as the repostDelay is specified
        /// before restarting
        /// </summary>
        private readonly TimeSpan repostDelay;

        private Stopwatch watch;

        #endregion

        #region Constuct / Dispose

        /// <summary>
        /// The task restart delay
        /// </summary>
        /// <param name="repostDelay">The delay</param>
        public ParallelTaskStarter(TimeSpan repostDelay)
        {
            this.repostDelay = repostDelay;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool isDisposed = false;
        private void Dispose(bool disposing)
        {
            if (disposing && !isDisposed)
            {
                wtoken.Dispose();
                isDisposed = true;
            }
        }

        #endregion
        
        #region Task Creator

        /// <summary>
        /// Creates the parallel task
        /// </summary>
        /// <param name="action">The action thats' being run</param>
        /// <param name="cancellationToken">The token that cancels the task</param>
        /// <returns>A targetblock</returns>
        private ITargetBlock<DateTimeOffset> CreateParallelTask(
           Func<DateTimeOffset, CancellationToken, Task> action,
           CancellationToken cancellationToken)
        {
            ActionBlock<DateTimeOffset> block = null;

            block = new ActionBlock<DateTimeOffset>(async now =>
            {
                await action(now, cancellationToken).
                      ConfigureAwait(false);
                watch.Restart();
                await Task.Delay(repostDelay, cancellationToken).
                      ConfigureAwait(false);

                block.Post(DateTimeOffset.Now);

            }, new ExecutionDataflowBlockOptions
            {
                CancellationToken = cancellationToken
            });

            return block;
        }

        #endregion


        #region Start / Stop

        private Action asyncAction;

        /// <summary>
        /// Starts a parallel task
        /// </summary>
        /// <param name="action">The action that's run in a separate thread</param>
        public void Start(Action action)
        {
            watch = Stopwatch.StartNew();
            wtoken = new CancellationTokenSource();
            this.asyncAction = action;
            task = (ActionBlock<DateTimeOffset>)CreateParallelTask((now, ct) => DoAsync(ct), wtoken.Token);

            task.Post(DateTimeOffset.Now);
        }

        public double ElapsedTime => watch.Elapsed.TotalSeconds;

        private Task DoAsync(CancellationToken cancellationToken)
        {
            return Task.Run(asyncAction); 
        }
        
        /// <summary>
        /// Stops the parallel task
        /// </summary>
        public void Stop()
        {
            using (wtoken)
            {
                wtoken.Cancel();
            }
            wtoken = null;
            task = null;
            watch.Stop();
        }

        #endregion

    }
}
