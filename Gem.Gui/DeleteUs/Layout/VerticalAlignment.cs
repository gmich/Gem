using Gem.Gui.Rendering;
using Microsoft.Xna.Framework;
using System;

namespace Gem.Gui.Layout
{

    public interface IVerticalAlignable : IAlignable { }
    public class VerticalAlignment
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
            public void Align(Region parent, Region region)
            {
                throw new NotImplementedException();
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
            public void Align(Region parent, Region region)
            {
                throw new NotImplementedException();
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
            public void Align(Region parent, Region region)
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Stretch

        private static Lazy<StretchAlignment> stretch = new Lazy<StretchAlignment>();
        public static IVerticalAlignable Stretch
        {
            get
            {
                return stretch.Value;
            }
        }
        private class StretchAlignment : IVerticalAlignable
        {
            public void Align(Region parent, Region region)
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region None

        public static IVerticalAlignable Manual
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
