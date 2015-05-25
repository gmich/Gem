using Gem.Gui.Rendering;
using Gem.Gui.Transformations;

namespace Gem.Gui.Alignment
{
    /// <summary>
    /// Determines how a transition between two regions is performed
    /// </summary>
    public interface IAlignmentTransition
    {
        ITransformation CreateTransition(Region oldRegion, Region newRegion);
    }
}
