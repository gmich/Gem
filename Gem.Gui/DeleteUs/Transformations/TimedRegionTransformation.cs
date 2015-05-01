using Gem.Gui.Controls;
using Gem.Gui.Elements;
using Gem.Gui.Rendering;
using Gem.Gui.Transformation;
using System;

namespace Gem.Gui.Transformations
{
    public class TimedRegionTransformation : ITransformation
    {

        #region Fields

        private readonly Action<Region> regionTransformer;
        private readonly double duration;
        private double timePassed = 0D;
        private bool enabled = false;

        #endregion

        public TimedRegionTransformation(double duration, Action<Region> regionTransformer)
        {
            this.regionTransformer = regionTransformer;
            this.duration = duration;
        }

        public bool Enabled
        {
            get { return (timePassed < duration); }
        }

        public void Transform(AControl element, double deltaTime)
        {
            timePassed += deltaTime;

            regionTransformer(element.Region);
        }
    }
}
