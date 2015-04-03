using Gem.Infrastructure.Logging;
using System.Diagnostics;

namespace Gem.Infrastructure.LogicalOperations
{
    /// <summary>
    /// A custom trace listener
    /// </summary>
    public class LogTraceListener : TraceListener
    {
        private readonly IAppender append;

        public LogTraceListener(IAppender append)
        {
            this.append = append;
        }
        public override void Write(string message)
        {
            append.Info(message);
        }

        public override void WriteLine(string message)
        {
            append.Info(message);
        }
    }
}