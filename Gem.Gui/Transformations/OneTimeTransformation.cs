using Gem.Gui.Controls;
using Gem.Gui.Rendering;
using System;

namespace Gem.Gui.Transformations
{
    /// <summary>
    /// A transformation that expires after a period of time
    /// </summary>
    public class OneTimeTransformation : ITransformation
    {

        #region Fields

        private readonly Action<IRenderable> transformer;
        private bool enabled = true;

        #endregion

        public OneTimeTransformation(Action<IRenderable> transformer)
        {
            this.transformer = transformer;
        }

        public bool Enabled
        {
            get { return enabled; }
        }

        public void Transform(IRenderable control, double deltaTime)
        {
            enabled = false;
            transformer(control);
        }
    }
}
