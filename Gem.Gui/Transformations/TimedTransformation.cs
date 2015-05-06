using Gem.Gui.Controls;
using Gem.Gui.Core.Styles;
using Gem.Gui.Rendering;
using Gem.Gui.Transformation;
using System;

namespace Gem.Gui.Transformations
{
    /// <summary>
    /// A transformation that expires after a period of time
    /// </summary>
    public class TimedTransformation : ITransformation
    {

        #region Fields

        private readonly Action<double, RenderParameters> regionTransformer;
        private readonly double duration;
        private double timePassed = 0D;
        private bool enabled = false;

        #endregion

        public TimedTransformation(double duration, Action<double, RenderParameters> regionTransformer)
        {
            this.regionTransformer = regionTransformer;
            this.duration = duration;
        }

        public bool Enabled
        {
            get { return (timePassed < duration); }
        }

        public void Transform(AControl control, double deltaTime)
        {
            timePassed += deltaTime;

            regionTransformer(deltaTime, control.RenderParameters);
        }
    }
}
