using System.Linq.Expressions;
using System.Reflection;

namespace Gem.Network.Extensions
{
    /// <summary>
    /// Labda expression utility class
    /// </summary>
    public static class LambdaExtensions
    {
        /// <summary>
        /// Analyzes a lambda expression and returns the method info
        /// </summary>
        /// <param name="expression">The expression to analyze</param>
        /// <returns>The method info</returns>
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
