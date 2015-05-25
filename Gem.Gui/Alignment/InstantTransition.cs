using Gem.Gui.Rendering;
using Gem.Gui.Transformations;

namespace Gem.Gui.Alignment
{
    /// <summary>
    /// An instant transition.
    /// </summary>
    public class InstantTransition : IAlignmentTransition
    {
        public ITransformation CreateTransition(Region oldRegion, Region newRegion)
        {
            if (oldRegion == newRegion) return new NoTransformation();
            return new PredicateTransformation(control => oldRegion == newRegion,
                                               (deltaTime, control) =>
                                               {
                                                   oldRegion.Position = newRegion.Position;
                                                   oldRegion.Size = newRegion.Size;
                                               });
        }
    }
}
