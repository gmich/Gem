using System;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Gem.Infrastructure.Assertions
{

    public static class DebugArgument
    {

        public static class Require
        {
            [Conditional(CompilationSymbol.Debug)]
            public static void NotNull<T>(Expression<Func<T>> parameter)
            {
                Argument.NotNull(parameter);
            }

            [Conditional(CompilationSymbol.Debug)]
            public static void NotNullOrEmpty<T>(Expression<Func<T>> parameter)
            {
                Argument.NotNullOrEmpty(parameter);
            }

            [Conditional(CompilationSymbol.Debug)]
            public static void That(Func<bool> condition)
            {
                Argument.That(condition);
            }

            [Conditional(CompilationSymbol.Debug)]
            public static void Invariant<T>(Expression<Func<T>> parameter, Func<bool> condition)
            {
                Argument.Invariant(parameter, condition);
            }
        }

        public static class Ensure
        {
            [Conditional(CompilationSymbol.Debug)]
            public static void NotNull<T>(Expression<Func<T>> parameter)
            {
                Argument.NotNull(parameter);
            }

            [Conditional(CompilationSymbol.Debug)]
            public static void NotNullOrEmpty<T>(Expression<Func<T>> parameter)
            {
                Argument.NotNullOrEmpty(parameter);
            }

            [Conditional(CompilationSymbol.Debug)]
            public static void That(Func<bool> condition)
            {
                Argument.That(condition);
            }

            [Conditional(CompilationSymbol.Debug)]
            public static void Invariant<T>(Expression<Func<T>> parameter, Func<bool> condition)
            {
                Argument.Invariant(parameter, condition);
            }
        }
    }
}

