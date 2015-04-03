using Castle.DynamicProxy;
using Gem.Infrastructure.Logging;
using System;

namespace Gem.Infrastructure.Aspects
{
    /// <summary>
    /// Trace's method invocations
    /// Attribute usage: [Intercept("LogicalOperation")]
    /// </summary>
    public class ExceptionInterceptor : IInterceptor
    {
        private readonly Func<Exception, string> handleException;
        private readonly IAppender appender;

        public ExceptionInterceptor(IAppender appender, Func<Exception, string> handleException)
        {
            this.appender = appender;
            this.handleException = handleException;
        }

        public void Intercept(IInvocation invocation)
        {
            try
            {
                invocation.Proceed();
            }
            catch (Exception ex)
            {
                appender.Error(handleException(ex));
            }
        }
    }
}