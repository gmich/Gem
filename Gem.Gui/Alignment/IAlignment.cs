using Gem.Gui.Rendering;

namespace Gem.Gui.Alignment
{
    /// <summary>
    /// Performs an alignment
    /// </summary>
    public interface IAlignment
    {
        /// <summary>
        /// Calculates the new child region
        /// </summary>
        /// <returns>The new child's region </returns>
        AlignmentResult Align(Region parent, Region child, Padding padding);
    }
}
