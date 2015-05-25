namespace Gem.Gui.Controls
{
    /// <summary>
    /// The aggregation token is used with a control in AggregationContext.
    /// Has the aggregation specific information for each entry
    /// </summary>
    public class AggregationToken
    {
        /// <summary>
        /// The entry's index. This index is equivalent to the AggregationContext's index
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// True for mouse over
        /// </summary>
        public bool HasHover { get; set; }

        /// <summary>
        /// True if the conditions allow mouse clicking
        /// </summary>
        public bool CanBeClicked { get; set; }

        /// <summary>
        /// The aggregator that gave focus to the entry
        /// </summary>
        public object GotFocusBy { get; set; }

        /// <summary>
        /// Checks if that instance is the one that gave focus
        /// </summary>
        /// <typeparam name="TModifier">The instance's type</typeparam>
        /// <param name="instance">The instance</param>
        /// <returns>True when the instance is the one that previously gave focus</returns>
        public bool HasGottenFocusBy<TModifier>(TModifier instance)
            where TModifier : class
        {
            return (GotFocusBy as TModifier) != null;
        }
    }
}
