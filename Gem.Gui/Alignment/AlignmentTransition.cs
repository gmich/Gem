using System;

namespace Gem.Gui.Alignment
{
    /// <summary>
    /// Helper static class for fluently using predefined IAlignmentTransitions
    /// </summary>
    public static class AlignmentTransition
    {
        private static Lazy<InstantTransition> _instant = new Lazy<InstantTransition>();
        public static IAlignmentTransition Instant
        {
            get { return _instant.Value; }
        }
    }
}
