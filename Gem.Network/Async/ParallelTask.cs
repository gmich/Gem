using Seterlund.CodeGuard;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Gem.Network.Async
{
    public class ParallelTaskStarter
    {

        #region Fields

        private CancellationTokenSource wtoken;

        private ActionBlock<DateTimeOffset> task;
    
        private readonly TimeSpan repostDelay;

        #endregion


        #region Constructor

        public ParallelTaskStarter(TimeSpan repostDelay)
        {
            this.repostDelay = repostDelay;
        }

        #endregion


        #region Task Creator

        private ITargetBlock<DateTimeOffset> CreateParallelTask(
           Func<DateTimeOffset, CancellationToken, Task> action,
           CancellationToken cancellationToken)
        {
            Guard.That(action).IsNotNull();

            ActionBlock<DateTimeOffset> block = null;

            block = new ActionBlock<DateTimeOffset>(async now =>
            {
                await action(now, cancellationToken).
                      ConfigureAwait(false);

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

        public void Start(Action action)
        {
            wtoken = new CancellationTokenSource();
            this.asyncAction = action;
            task = (ActionBlock<DateTimeOffset>)CreateParallelTask((now, ct) => DoAsync(ct), wtoken.Token);

            task.Post(DateTimeOffset.Now);
        }

        private Task DoAsync(CancellationToken cancellationToken)
        {
            return Task.Run(asyncAction); 
        }
        
        public void Stop()
        {
            using (wtoken)
            {
                wtoken.Cancel();
            }
            wtoken = null;
            task = null;
        }

        #endregion

    }
}
