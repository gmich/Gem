using Gem.Gui.Rendering;
using System;

namespace Gem.Gui.Layout
{
    public class LayoutStyle
    {
        public IHorizontalAlignable HorizontalAlignment { get; set; }
        public IVerticalAlignable VerticalAlignment { get; set; }

        private static Lazy<LayoutStyle> defaultOptions = new Lazy<LayoutStyle>(() =>
            new LayoutStyle
            {
                HorizontalAlignment = Layout.HorizontalAlignment.None,
                VerticalAlignment = Layout.VerticallAlignment.None
            });

        public static LayoutStyle Default
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
