using Gem.Gui.Rendering;

namespace Gem.Gui.Alignment
{
    /// <summary>
    /// Classes that implement the IAlignable are applicable for aligning
    /// </summary>
    public interface IAlignable
    {
        void Align(Region parent);
    }
}
