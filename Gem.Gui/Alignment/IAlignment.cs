using Gem.Gui.Rendering;

namespace Gem.Gui.Alignment
{
    public interface IAlignment
    {
        /// <summary>
        /// Calculates the new child region
        /// </summary>
        /// <returns>The new child's region </returns>
        AlignmentResult Align(Region parent, Region child, Padding padding);
    }
}
