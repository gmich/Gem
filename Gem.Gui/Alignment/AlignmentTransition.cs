using System;

namespace Gem.Gui.Alignment
{
    public sealed class AlignmentTransition
    {

        private static Lazy<FixedTransition> _fixed = new Lazy<FixedTransition>();
        public static IAlignmentTransition Fixed
        {
            get { return _fixed.Value; }
        }

    }
}
