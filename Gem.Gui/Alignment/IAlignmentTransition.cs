using Gem.Gui.Rendering;
using Gem.Gui.Transformations;

namespace Gem.Gui.Alignment
{
    public interface IAlignmentTransition
    {
        ITransformation CreateTransition(Region oldRegion, Region newRegion);
    }
}
