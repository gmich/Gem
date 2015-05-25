using System;

namespace Gem.Gui.Aggregation
{
    /// <summary>
    /// AggregationTarget shows which input capabilties the Gui is targeting
    /// </summary>
    [Flags]
    public enum AggregationTarget
    {
        None = 0,
        Mouse = 1,
        Keyboard = 2,
        GamePad = 4,
        Touch = 8,
        All = Mouse | Keyboard | GamePad | Touch
    }

    public static class AggregationTargetExtensions
    {
        /// <summary>
        /// Helper extension for AggregationTarget enum for checking if an instance has a specific flag.
        /// This is a Enum.HasFlag duplicate, but AggregationTarget specific, because HasFlag is has performance costs 
        /// </summary>
        public static bool HasTargetFlag(this AggregationTarget target, AggregationTarget flag)
        {
            return ((target & flag) != 0);
        }
    }
}
