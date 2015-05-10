using Gem.Gui.Controls;
using Gem.Gui.Core.Styles;
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

        private readonly Action<double, AControl> transformer;
        private readonly Predicate<AControl> expirationPredicate;
        private bool enabled = false;

        #endregion

        public PredicateTransformation(Predicate<AControl> expirationPredicate, Action<double, AControl> transformer)
        {
            this.transformer = transformer;
            this.expirationPredicate = expirationPredicate;
        }

        public bool Enabled
        {
            get { return enabled; }
        }

        public void Transform(AControl control, double deltaTime)
        {
            if (enabled = expirationPredicate(control))
            {
                transformer(deltaTime, control);
            }
        }
    }
}
