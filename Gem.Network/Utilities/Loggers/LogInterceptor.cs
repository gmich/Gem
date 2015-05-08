using System;
using Castle.DynamicProxy;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Gem.Network.Utilities.Loggers
{
    /// <summary>
    /// Intercepts methods to log prior-invocation , post invocation and on exception
    /// </summary>
    public class LogInterceptor : IInterceptor
    {

        private readonly IAppender auditor;

        public LogInterceptor(IAppender auditor)
        {
            this.auditor = auditor;
        }

        public void Intercept(IInvocation invocation)
        {
            //prior invocation
            auditor.Debug("Calling {0}", GetMethodInformation(invocation));

            try
            {
                invocation.Proceed();

                //post invocation
                auditor.Debug("Done: result was {0}.", invocation.ReturnValue);
            }
            catch (Exception ex)
            {
                //on exception
                auditor.Error("Threw exception {0} . {1} ", ex.Message, GetMethodInformation(invocation));
                throw;
            }
        }

        private string GetMethodInformation(IInvocation invocation)
        {
            return String.Format(
              "Method {0} with parameters {1}... ",
              invocation.Method.Name,
              string.Join(", ", invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray()));
        }

        private string CreateInvocationLogString(IInvocation invocation)
        {
            StringBuilder sb = new StringBuilder(100);
            sb.AppendFormat("Called: {0}.{1}(", invocation.TargetType.Name, invocation.Method.Name);
            foreach (object argument in invocation.Arguments)
            {
                String argumentDescription = argument == null ? "null" : DumpObject(argument);
                sb.Append(argumentDescription).Append(",");
            }
            if (invocation.Arguments.Count() > 0) sb.Length--;
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

    }
}