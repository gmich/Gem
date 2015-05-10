using Gem.Gui.Rendering;

namespace Gem.Gui.Alignment
{
    public interface IAlignable
    {
        /// <summary>
        /// Calculates the new child region
        /// </summary>
        /// <returns>The new child's region </returns>
        Region Align(Region parent, Region child, Padding padding);
    }
}
