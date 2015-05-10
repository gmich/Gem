using Gem.Gui.Controls;

namespace Gem.Gui.Transformations
{
    public interface ITransformation
    {
        bool Enabled { get; }

        void Transform(AControl element, double deltaTime);
    }

}
