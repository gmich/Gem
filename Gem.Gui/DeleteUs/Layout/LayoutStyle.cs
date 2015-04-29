using Gem.Gui.Rendering;
using System;

namespace Gem.Gui.Layout
{
    public class Alignment
    {
        public IHorizontalAlignable HorizontalAlignment { get; set; }
        public IVerticalAlignable VerticalAlignment { get; set; }

        private static Lazy<Alignment> defaultOptions = new Lazy<Alignment>(() =>
            new Alignment
            {
                HorizontalAlignment = Layout.HorizontalAlignment.Manual,
                VerticalAlignment = Layout.VerticalAlignment.Manual
            });

        public static Alignment Default
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
