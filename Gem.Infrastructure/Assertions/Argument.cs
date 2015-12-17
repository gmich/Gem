using System;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Gem.Infrastructure.Assertions
{

    public static class Argument
    {
        private static void ThrowArgumentException(string parameterName, string message)
        {
            var stackTrace = new StackTrace(true);
            var stackFrames = stackTrace.GetFrames();
            throw new ArgumentException(
                parameterName,
                $"{message} Function: { stackFrames[2].GetMethod().Name}");
        }

        internal static void Invariant<T>(Expression<Func<T>> parameter, Func<bool> condition)
        {
            var memberExpression = parameter.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new ApplicationException(
                    "Can only use a member expression with DebugArgument.NotNull.");
            }

            if (!condition())
            {
                ThrowArgumentException(
                    memberExpression.Member.Name,
                    $"{memberExpression.Type.Name} parameter failed invariant condition. ");
            }
        }
        internal static void That(Func<bool> condition)
        {
            if (!condition())
            {
                ThrowArgumentException("Should condition", "Method didn't pass the condition. ");
            }
        }

        internal static void NotNull<T>(Expression<Func<T>> parameter)
        {
            var memberExpression = parameter.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new ApplicationException(
                    "Can only use a member expression with DebugArgument.NotNull.");
            }

            var value = parameter.Compile().Invoke();
            if (value == null)
            {
                ThrowArgumentException(memberExpression.Member.Name,
                     $"Parameter type: {memberExpression.Type.Name} ");
            }
        }

        internal static void NotNullOrEmpty<T>(Expression<Func<T>> parameter)
        {
            var memberExpression = parameter.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new ApplicationException(
                    "Can only use a member expression with DebugArgument.StringNotNullOrEmpty.");
            }

            if (memberExpression.Type != typeof(String))
            {
                throw new ApplicationException(
                    "StringNotNullOrEmpty can only be used with string arguments, type was " + memberExpression.Type.Name);
            }

            var value = parameter.Compile().Invoke() as string;
            if (value == null)
            {
                ThrowArgumentException(
                    memberExpression.Member.Name,
                    $"Parameter type: {memberExpression.Type.Name} ");
            }

            if (value == string.Empty)
            {
                ThrowArgumentException(
                   memberExpression.Member.Name,
                   "Empty string parameter. ");
            }
        }

        internal static void ArrayIndex(Expression<Func<int>> parameter)
        {
            var memberExpression = parameter.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new ApplicationException(
                    "Can only use a member expression with DebugArgument.ArrayIndex.");
            }

            if (memberExpression.Type != typeof(int))
            {
                throw new ApplicationException(
                    $"ArrayIndex can only be used with int arguments, type was {memberExpression.Type.Name}");
            }

            var value = parameter.Compile().Invoke();
            if (value < 0)
            {
                ThrowArgumentException(
                    memberExpression.Member.Name,
                   "Negative array index is invalid");
            }
        }

        internal static void ArrayIndex(Expression<Func<int>> parameter, int maxArrayElements)
        {
            var memberExpression = parameter.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new ApplicationException(
                    "Can only use a member expression with DebugArgument.ArrayIndex.");
            }

            if (memberExpression.Type != typeof(int))
            {
                throw new ApplicationException(
                    $"ArrayIndex can only be used with int arguments, type was {memberExpression.Type.Name} ");
            }

            var value = parameter.Compile().Invoke();
            if (value < 0)
            {
                ThrowArgumentException(
                    memberExpression.Member.Name,
                    $"Negative array index is invalid. Index: {value}");
            }
            else if (value >= maxArrayElements)
            {
                ThrowArgumentException(
                    memberExpression.Member.Name,
                    $"Array access out of bounds index is invalid. Max should be: {maxArrayElements} .");
            }
        }

        public static class Require
        {
            public static void NotNull<T>(Expression<Func<T>> parameter)
            {
                Argument.NotNull(parameter);
            }

            public static void NotNullOrEmpty<T>(Expression<Func<T>> parameter)
            {
                Argument.NotNullOrEmpty(parameter);
            }

            public static void That(Func<bool> condition)
            {
                Argument.That(condition);
            }

            public static void Invariant<T>(Expression<Func<T>> parameter, Func<bool> condition)
            {
                Argument.Invariant(parameter, condition);
            }
        }

        public static class Ensure
        {
            public static void NotNull<T>(Expression<Func<T>> parameter)
            {
                Argument.NotNull(parameter);
            }

            public static void NotNullOrEmpty<T>(Expression<Func<T>> parameter)
            {
                Argument.NotNullOrEmpty(parameter);
            }

            public static void That(Func<bool> condition)
            {
                Argument.That(condition);
            }

            public static void Invariant<T>(Expression<Func<T>> parameter, Func<bool> condition)
            {
                Argument.Invariant(parameter, condition);
            }
        }
    }
}

