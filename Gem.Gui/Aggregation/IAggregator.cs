using System;

namespace Gem.Gui.Aggregation
{
    /// <summary>
    /// IAggregator is responsible for enumerating controls, firing events, check for focus, hover etc..
    /// </summary>
    public interface IAggregator
    {
        /// <summary>
        /// Shows is the current aggregator aggregates
        /// </summary>
        bool IsEnabled { get; }

        /// <summary>
        /// Performs the aggregation for an entry at the given context
        /// </summary>
        /// <param name="entry">The entry to be aggregated</param>
        /// <param name="context">The context</param>
        void Aggregate(AggregationEntry entry, AggregationContext context);
    }
}
