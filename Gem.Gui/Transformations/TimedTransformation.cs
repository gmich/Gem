using Gem.Gui.Rendering;
using System;

namespace Gem.Gui.Transformations
{
    /// <summary>
    /// A transformation that expires after a period of time
    /// </summary>
    public class TimedTransformation : ITransformation
    {

        #region Fields

        private readonly Action<double, IRenderable> transformer;
        private readonly double duration;
        private double timePassed = 0D;

        #endregion

        public TimedTransformation(double duration, Action<double, IRenderable> transformer)
        {
            this.transformer = transformer;
            this.duration = duration;
        }

        public bool Enabled
        {
            get { return (timePassed < duration); }
        }

        public void Transform(IRenderable control, double deltaTime)
        {
            timePassed += deltaTime;

            transformer(deltaTime, control);
        }
    }
}
