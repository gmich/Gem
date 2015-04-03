using Gem.Infrastructure.Logging;
using System;
using System.Diagnostics;

namespace Gem.Infrastructure.LogicalOperations
{
    /// <summary>
    /// Correlates traces that are part of a logical transaction
    /// </summary>
    public class CorrelationTrace
    {
        private readonly TraceSource ts;

        public CorrelationTrace(IAppender appender, string tracedApp)
        {
            ts = new TraceSource(tracedApp);
            int listenerIndex = ts.Listeners.Add(new LogTraceListener(appender));
            ts.Listeners[listenerIndex].TraceOutputOptions = TraceOptions.LogicalOperationStack;
            ts.Switch = new SourceSwitch(tracedApp, "Verbose");
        }

        public void Start(TraceEventType traceType, int traceId, string tracedThread)
        {
            Trace.CorrelationManager.StartLogicalOperation(tracedThread);
            Trace.CorrelationManager.StartLogicalOperation(tracedThread);
            ts.TraceEvent(traceType,
                          traceId,
                          String.Format("{0} in thread {1}", traceType, tracedThread));
        }

        public void Stop()
        {
            Trace.CorrelationManager.StopLogicalOperation();
        }
    }
}