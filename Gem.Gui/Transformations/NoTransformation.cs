using Gem.Gui.Controls;
using Gem.Gui.Rendering;

namespace Gem.Gui.Transformations
{
    public class NoTransformation : ITransformation
    {
        public bool Enabled { get { return false; } }

        public void Transform(IRenderable element, double deltaTime) { return; }
    }

}
