using Gem.Gui.Rendering;
using System;

namespace Gem.Gui.Alignment
{
    //For type safety
    public interface IHorizontalAlignable : IAlignable {  }

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
            public void Align(Region parent, Region region)
            {
                throw new NotImplementedException();
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
            public void Align(Region parent, Region region)
            {
                throw new NotImplementedException();
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
            public void Align(Region parent, Region region)
            {
                throw new NotImplementedException();
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
            public void Align(Region parent, Region region)
            {
                throw new NotImplementedException();
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
            public void Align(Region parent, Region region)
            {
                return;
            }
        }

        #endregion

    }
}
