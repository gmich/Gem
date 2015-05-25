using Gem.Gui.Controls;

namespace Gem.Gui.Aggregation
{
    /// <summary>
    /// The entry for AggregationContext
    /// </summary>
    public class AggregationEntry
    {
        private readonly AControl control;

        /// <summary>
        /// The entry's index. This index is equivalent to the AggregationContext's index
        /// </summary>
        private readonly int index;

        public AggregationEntry(AControl control, int index)
        {
            this.control = control;
            this.index = index;
            Token = new AggregationToken();
        }

        /// <summary>
        /// The entry's index. This index is equivalent to the AggregationContext's index
        /// </summary>
        public int Index { get { return index; } }

        public AControl Control { get { return control; } }

        public AggregationToken Token { get; set; }
    }
}
