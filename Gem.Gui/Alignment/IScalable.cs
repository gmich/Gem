using Gem.Gui.Rendering;
using Microsoft.Xna.Framework;

namespace Gem.Gui.Alignment
{
    /// <summary>
    /// Classes that implement the IScalable are applicable for scaling
    /// </summary>
    public interface IScalable
    {
        void Scale(Vector2 scale);
    }
}
