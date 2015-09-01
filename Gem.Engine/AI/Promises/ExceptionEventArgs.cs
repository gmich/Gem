using System;
using Gem.Infrastructure.Debug;

namespace Gem.AI.Promises
{

    /// <summary>
    /// Arguments to the UnhandledError event.
    /// </summary>
    public class ExceptionEventArgs : EventArgs
    {
        internal ExceptionEventArgs(Exception exception)
        {
            Argument.NotNull(() => exception);

            this.Exception = exception;
        }

        public Exception Exception
        {
            get;
            private set;
        }
    }
}
