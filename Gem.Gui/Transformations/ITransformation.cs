using Gem.Gui.Controls;
using Gem.Gui.Rendering;

namespace Gem.Gui.Transformations
{
    public interface ITransformation
    {
        bool Enabled { get; }

        void Transform(IRenderable element, double deltaTime);
    }

}
