using Gem.Gui.Rendering;
using Microsoft.Xna.Framework;

namespace Gem.Gui.Layout
{
    public interface IAlignable
    {
        void Align(Region parent, Region region);
    }
}
