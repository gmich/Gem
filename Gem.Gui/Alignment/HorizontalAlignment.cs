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
            public AlignmentResult Align(Region parent, Region child, Padding padding)
            {
                return new AlignmentResult(parent.Position.X + padding.Left, child.Size.X);
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
            public AlignmentResult Align(Region parent, Region child, Padding padding)
            {
                float centerX = parent.Position.X + parent.Size.X / 2;
                return new AlignmentResult(centerX - child.Size.X / 2, child.Size.X);
            }
        }

        #endregion

        #region Right

        private static Lazy<RightAlignment> right = new Lazy<RightAlignment>();
        public static IHorizontalAlignable Right
        {
            get
            {
                return right.Value;
            }
        }
        private class RightAlignment : IHorizontalAlignable
        {
            public AlignmentResult Align(Region parent, Region child, Padding padding)
            {
                return new AlignmentResult(parent.Frame.Right - child.Size.X - padding.Right, child.Size.X);
            }
        }

        #endregion

        #region Stretch

        //private static Lazy<StretchAlignent> stretch = new Lazy<StretchAlignent>();
        //public static IHorizontalAlignable Stretch
        //{
        //    get
        //    {
        //        return stretch.Value;
        //    }
        //}
        //private class StretchAlignent : IHorizontalAlignable
        //{
        //    public AlignmentResult Align(Region parent, Region child, Padding padding)
        //    {
        //        return new AlignmentResult(parent.Position.X + padding.Left,
        //                                   parent.Size.X - padding.Right);
        //    }
        //}

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
            public AlignmentResult Align(Region parent, Region child, Padding padding)
            {
                return new AlignmentResult(child.Position.X, child.Size.X);
            }
        }

        #endregion

    }
}
