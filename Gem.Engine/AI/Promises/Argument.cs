using System;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Gem.AI.Promises
{
    public sealed class Argument
    {

        [Conditional("DEBUG")]
        public static void NotNull<T>(Expression<Func<T>> parameter)
        {
            var memberExpression = parameter.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new ApplicationException("Can only use a member expression with Argument.NotNull.");
            }

            var value = parameter.Compile().Invoke();
            if (value == null)
            {
                var parameterName = memberExpression.Member.Name;
                var stackTrace = new StackTrace(true);
                var stackFrames = stackTrace.GetFrames();
                var msg = "Parameter type: " + memberExpression.Type.Name + ", Function: " + stackFrames[1].GetMethod().Name;
                throw new ArgumentNullException(parameterName, msg);
            }
        }

    }
}
