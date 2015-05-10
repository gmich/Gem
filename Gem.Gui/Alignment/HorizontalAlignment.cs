using Gem.Gui.Rendering;
using Microsoft.Xna.Framework;
using System;

namespace Gem.Gui.Alignment
{
    //For type safety
    public interface IHorizontalAlignable : IAlignable { }

    public class HorizontalAlignment
    {
        #region Left

        private static Lazy<LeftAlignment> left = new Lazy<LeftAlignment>();
        public static IHorizontalAlignable Left
        {
            get
            {
                return left.Value;
            }
        }

        private class LeftAlignment : IHorizontalAlignable
        {
            public Region Align(Region parent, Region child, Padding padding)
            {
                return new Region(new Vector2(parent.Position.X + padding.Left, child.Position.Y), child.Size);
            }
        }

        #endregion

        #region Center

        private static Lazy<CenterAlignent> center = new Lazy<CenterAlignent>();
        public static IHorizontalAlignable Center
        {
            get
            {
                return center.Value;
            }
        }
        private class CenterAlignent : IHorizontalAlignable
        {
            public Region Align(Region parent, Region child, Padding padding)
            {
                float centerX = parent.Position.X + parent.Size.X / 2;
                return new Region(new Vector2(centerX - child.Size.X / 2, child.Position.Y), child.Size);
            }
        }

        #endregion

        #region Right

        private static Lazy<RightAlignment> right = new Lazy<RightAlignment>();
        public static IAlignable Right
        {
            get
            {
                return right.Value;
            }
        }
        private class RightAlignment : IAlignable
        {
            public Region Align(Region parent, Region child, Padding padding)
            {
                return new Region(new Vector2(parent.Position.X - child.Size.X - padding.Right, child.Position.Y), child.Size);
            }
        }

        #endregion

        #region Stretch

        private static Lazy<StretchAlignent> stretch = new Lazy<StretchAlignent>();
        public static IHorizontalAlignable Stretch
        {
            get
            {
                return stretch.Value;
            }
        }
        private class StretchAlignent : IHorizontalAlignable
        {
            public Region Align(Region parent, Region child, Padding padding)
            {
                return new Region(new Vector2(parent.Position.X + padding.Left, child.Position.Y),
                                  new Vector2(parent.Size.X - padding.Right, child.Size.Y));
            }
        }

        #endregion

        #region Manual

        private static Lazy<ManualAlignent> manual = new Lazy<ManualAlignent>();
        public static IHorizontalAlignable Manual
        {
            get
            {
                return manual.Value;
            }
        }
        private class ManualAlignent : IHorizontalAlignable
        {
            public Region Align(Region parent, Region child, Padding padding)
            {
                return child;
            }
        }

        #endregion

    }
}
