using Gem.Gui.Controls;
using Gem.Gui.Rendering;
using System;

namespace Gem.Gui.Alignment
{
    //for type safety
    public interface IVerticalAlignable : IAlignment { }

    /// <summary>
    /// Holds all the Vertical Alignment related classes
    /// </summary>
    public static class VerticalAlignment
    {
        #region Top

        private static Lazy<TopAlignment> top = new Lazy<TopAlignment>();
        public static IVerticalAlignable Top
        {
            get
            {
                return top.Value;
            }
        }

        private class TopAlignment : IVerticalAlignable
        {
            public AlignmentResult Align(Region parent, Region child, Padding padding)
            {
                return new AlignmentResult(parent.Frame.Top + padding.Top, child.Size.Y);
            }
        }

        #endregion

        #region Center

        private static Lazy<CenterAlignment> center = new Lazy<CenterAlignment>();
        public static IVerticalAlignable Center
        {
            get
            {
                return center.Value;
            }
        }

        private class CenterAlignment : IVerticalAlignable
        {
            public AlignmentResult Align(Region parent, Region child, Padding padding)
            {
                float centerY = parent.Position.Y + parent.Size.Y / 2;
                return new AlignmentResult(centerY - child.Size.Y / 2, child.Size.Y);
            }
        }

        #endregion

        #region Bottom

        private static Lazy<BottomAlignment> bottom = new Lazy<BottomAlignment>();
        public static IVerticalAlignable Bottom
        {
            get
            {
                return bottom.Value;
            }
        }

        private class BottomAlignment : IVerticalAlignable
        {
            public AlignmentResult Align(Region parent, Region child, Padding padding)
            {
                return new AlignmentResult(parent.Frame.Bottom - child.Size.Y - padding.Bottom, child.Size.Y);

            }
        }

        #endregion

        #region Stretch

        //private static Lazy<StretchAlignment> stretch = new Lazy<StretchAlignment>();
        //public static IVerticalAlignable Stretch
        //{
        //    get
        //    {
        //        return stretch.Value;
        //    }
        //}

        //private class StretchAlignment : IVerticalAlignable
        //{
        //    public AlignmentResult Align(Region parent, Region child, Padding padding)
        //    {
        //        return new AlignmentResult(parent.Position.Y + padding.Top,
        //                                   parent.Size.Y - padding.Right);
        //    }
        //}

        #endregion

        #region Manual

        private static Lazy<ManualAlignment> manual = new Lazy<ManualAlignment>();
        public static IVerticalAlignable Manual
        {
            get
            {
                return manual.Value;
            }
        }

        private class ManualAlignment : IVerticalAlignable
        {
            public AlignmentResult Align(Region parent, Region child, Padding padding)
            {
                return new AlignmentResult(child.Position.Y, child.Size.Y);
            }

        }
        #endregion

        #region Relative

        public static IVerticalAlignable RelativeTo(Func<float> relativeX)
        {

            return new VerticalRelative(relativeX);
        }

        public static IVerticalAlignable VerticalRelativeTo(this AControl control, AControl relativeControl, Func<float> relativeX)
        {
            if (relativeControl != null)
            {
                relativeControl.Region.onPositionChange += (sender, args) => control.Align(Configuration.Settings.ViewRegion);
                relativeControl.Region.onSizeChange += (sender, args) => control.Align(Configuration.Settings.ViewRegion);
            }
            return new VerticalRelative(relativeX);
        }

        private class VerticalRelative : IVerticalAlignable
        {
            private readonly Func<float> relativeX;

            public VerticalRelative(Func<float> relativeX)
            {
                this.relativeX = relativeX;
            }

            public AlignmentResult Align(Region parent, Region child, Padding padding)
            {
                return new AlignmentResult(relativeX(), child.Size.Y);
            }
        }

        #endregion
    }
}
