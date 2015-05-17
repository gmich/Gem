using Gem.Gui.Controls;
using Gem.Gui.Styles;
using Gem.Gui.Rendering;
using System;

namespace Gem.Gui.Transformations
{
    /// <summary>
    /// A transformation that expires after a predicate match
    /// </summary>
    public class PredicateTransformation : ITransformation
    {

        #region Fields

        private readonly Action<double, IRenderable> transformer;
        private readonly Predicate<IRenderable> expirationPredicate;
        private bool enabled = true;

        #endregion

        public PredicateTransformation(Predicate<IRenderable> expirationPredicate, Action<double, IRenderable> transformer)
        {
            this.transformer = transformer;
            this.expirationPredicate = expirationPredicate;
        }

        public bool Enabled
        {
            get { return enabled; }
        }

        public void Transform(IRenderable control, double deltaTime)
        {
            if (enabled = !expirationPredicate(control))
            {
                transformer(deltaTime, control);
            }
        }
    }
}
