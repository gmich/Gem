using Gem.Gui.Controls;
using Gem.Gui.Elements;

namespace Gem.Gui.Transformation
{
    public interface ITransformation
    {
        bool Enabled { get; }

        void Transform(AControl element, double deltaTime);
    }

}
