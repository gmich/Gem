using Castle.DynamicProxy;
using Gem.Infrastructure.LogicalOperations;
using System;
using System.Linq;

namespace Gem.Infrastructure.Aspects
{
    /// <summary>
    /// Trace's method invocations
    /// Attribute usage: [Intercept("LogicalOperation")]
    /// </summary>
    public class LogicalOperationInterceptor : IInterceptor
    {
        public LogicalOperationInterceptor()
        { }

        public void Intercept(IInvocation invocation)
        {
            using (OperationStack.Push(GetMethodInformation(invocation)))
            {
                invocation.Proceed();
            }
        }

        private string GetMethodInformation(IInvocation invocation)
        {
            return String.Format(
              "{0} ( {1} ) ",
              invocation.Method.Name,
              string.Join(", ", invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray()));
        }
    }
}