using System;
using System.Collections.Generic;

namespace Gem.Infrastructure.Exceptions
{
    /// <summary>
    /// Utility class for exception handling
    /// </summary>
    public sealed class ExceptionUtilities
    {
        /// <summary>
        /// Checks if the Aggregate Exception has inner exception of type TInner.
        /// If an exception is found, TInner's message is passed in exceptionMessageHandler().
        /// If TInner is not found, return false.
        /// </summary>
        /// <remarks>TInner must be contravariant to Exception</remarks>
        /// <typeparam name="TInner">The AggregateException's inner exception to look for</typeparam>
        /// <param name="exception">The Aggregate Exception</param>
        /// <param name="exceptionMessageHandler">The inner exception's message handler</param>
        /// <returns>
        /// <value>True if TInner is in AggregateException's inner stack</returns>
        public static bool HandleAggregateInner<TInner>(AggregateException exception, Action<string> exceptionMessageHandler)
        {
            var handlers = new Dictionary<Type, Action<Exception>>();
            handlers.Add(typeof(TInner), ex => exceptionMessageHandler(ex.Message));
            
            return (!HandleAggregateError(exception, handlers));
        }
        
        /// <summary>
        /// Checks recursively if the exception is contained in the exception handler. 
        /// Returns false if the exception is not handled correctly
        /// </summary>
        /// <param name="aggregate">The aggregation exception to check recursively</param>
        /// <param name="exceptionHandlers">A dictionary holing the exception type and its handler</param>
        /// <returns>False if the exception cannot be processed correctly</returns>
        private static bool HandleAggregateError(AggregateException aggregate,
                                                 Dictionary<Type, Action<Exception>> exceptionHandlers)
        {
            foreach (var exception in aggregate.InnerExceptions)
            {
                if (exception is AggregateException)
                {
                    return HandleAggregateError(exception as AggregateException,
                                                exceptionHandlers);
                }
                else if (exceptionHandlers.ContainsKey(exception.GetType()))
                {
                    exceptionHandlers[exception.GetType()]
                                     (exception);
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
    }
}
