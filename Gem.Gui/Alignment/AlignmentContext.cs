using Gem.Gui.Rendering;
using System;

namespace Gem.Gui.Alignment
{
    public class AlignmentContext
    {
        public IHorizontalAlignable HorizontalAlignment { get; set; }
        public IVerticalAlignable VerticalAlignment { get; set; }

        private static Lazy<AlignmentContext> defaultOptions = new Lazy<AlignmentContext>(() =>
            new AlignmentContext
            {
                HorizontalAlignment = Alignment.HorizontalAlignment.Manual,
                VerticalAlignment = Alignment.VerticalAlignment.Manual
            });

        public static AlignmentContext Default
        {
            get { return defaultOptions.Value; }
        }

        internal void AdjustLocation(Region parent, Region current)
        {
            HorizontalAlignment.Align(parent, current);
            VerticalAlignment.Align(parent, current);
        }
    }
}
