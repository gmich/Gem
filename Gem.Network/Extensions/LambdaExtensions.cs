using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Gem.Network.Extensions
{
    public static class LambdaExtensions
    {
        public static MethodInfo GetMethodInfo(this LambdaExpression expression)
        {
            var lambdaExpression = (LambdaExpression)expression;
            var unaryExpression = (UnaryExpression)lambdaExpression.Body;
            var methodCallExpression = (MethodCallExpression)unaryExpression.Operand;
            var methodInfoExpression = (ConstantExpression)methodCallExpression.Object;

            return (MethodInfo)methodInfoExpression.Value;           
        }
    }
}
