using System;
using Castle.DynamicProxy;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Gem.Infrastructure.Logging;

namespace Gem.Infrastructure.Aspects
{
    /// <summary>
    /// Methods / Interfaces / Classes intercepted are logged detailed
    /// Attribute usage: [Intercept("Log")]
    /// </summary>
    public class LogInterceptor : IInterceptor
    {
        private readonly IAppender appender;

        public LogInterceptor(IAppender appender)
        {
            this.appender = appender;
        }

        public void Intercept(IInvocation invocation)
        {
            //Prior-invocation
            appender.Debug("Invoking {0}", GetMethodInformation(invocation));

            try
            {
                invocation.Proceed();
                //Post-invocation
                appender.Debug("{0} has finished. Returned {1}",invocation.Method.Name, invocation.ReturnValue);
            }
            catch (Exception ex)
            {
                //On exception
                appender.Error("Threw exception {0} . {1} ", ex.Message, GetMethodInformation(invocation));
                throw ex;
            }
        }

        #region Log Dumbing Helpers

        private string GetMethodInformation(IInvocation invocation)
        {
            return String.Format(
              " {0} ( {1} )",
              invocation.Method.Name,
              string.Join(", ", invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray()));
        }

        private string CreateInvocationLogString(IInvocation invocation)
        {
            var sb = new StringBuilder(100);
            sb.AppendFormat("Called: {0}.{1}(", invocation.TargetType.Name, invocation.Method.Name);
            foreach (object argument in invocation.Arguments)
            {
                String argumentDescription = argument == null ? "null" : DumpObject(argument);
                sb.Append(argumentDescription).Append(",");
            }
            if (invocation.Arguments.Count() > 0)
            {
                sb.Length--;
            }
            sb.Append(")");
            return sb.ToString();
        }

        private string DumpObject(object argument)
        {
            var sb = new StringBuilder();
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(argument))
            {
                string name = descriptor.Name;
                object value = descriptor.GetValue(argument);
                sb.AppendLine(String.Format("{0}={1}", name, value));
            }
            return sb.ToString();
        }

        #endregion

    }
}