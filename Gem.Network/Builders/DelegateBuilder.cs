using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Gem.Network.Builders
{
    /// <summary>
    /// Helper class for runtime delegate building
    /// </summary>
    internal class DelegateBuilder
    {
        /// <summary>
        /// Builds a delegate
        /// </summary>
        /// <typeparam name="T">The class that the delegate is a member of</typeparam>
        /// <param name="method">The method info</param>
        /// <param name="missingParamValues">The parameters</param>
        /// <returns>The class' new delegate</returns>
        internal static T BuildDelegate<T>(MethodInfo method, params object[] missingParamValues)
        {
            var queueMissingParams = new Queue<object>(missingParamValues);

            var dgtMi = typeof(T).GetMethod("Invoke");
            var dgtRet = dgtMi.ReturnType;
            var dgtParams = dgtMi.GetParameters();

            var paramsOfDelegate = dgtParams
                .Select(tp => Expression.Parameter(tp.ParameterType, tp.Name))
                .ToArray();

            var methodParams = method.GetParameters();

            if (method.IsStatic)
            {
                var paramsToPass = methodParams
                    .Select((p, i) => CreateParam(paramsOfDelegate, i, p, queueMissingParams))
                    .ToArray();

                var expr = Expression.Lambda<T>(
                    Expression.Call(method, paramsToPass),
                    paramsOfDelegate);

                return expr.Compile();
            }
            else
            {
                var paramThis = Expression.Convert(paramsOfDelegate[0], method.DeclaringType);

                var paramsToPass = methodParams
                    .Select((p, i) => CreateParam(paramsOfDelegate, i + 1, p, queueMissingParams))
                    .ToArray();

                var expr = Expression.Lambda<T>(
                    Expression.Call(paramThis, method, paramsToPass),
                    paramsOfDelegate);

                return expr.Compile();
            }
        }

        /// <summary>
        /// Helper class that creates runtime parameters for delegates
        /// </summary>
        /// <param name="paramsOfDelegate">The parameters</param>
        /// <param name="i">The index</param>
        /// <param name="callParamType">The type</param>
        /// <param name="queueMissingParams">The rest parameters</param>
        /// <returns>An expression thats used for runtime delegate creation</returns>
        private static Expression CreateParam(ParameterExpression[] paramsOfDelegate, int i, ParameterInfo callParamType, Queue<object> queueMissingParams)
        {
            if (i < paramsOfDelegate.Length)
                return Expression.Convert(paramsOfDelegate[i], callParamType.ParameterType);

            if (queueMissingParams.Count > 0)
                return Expression.Constant(queueMissingParams.Dequeue());

            if (callParamType.ParameterType.IsValueType)
                return Expression.Constant(Activator.CreateInstance(callParamType.ParameterType));

            return Expression.Constant(null);
        }
    }
}

