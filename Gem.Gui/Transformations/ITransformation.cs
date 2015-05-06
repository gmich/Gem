using Gem.Gui.Controls;

namespace Gem.Gui.Transformation
{
    public interface ITransformation
    {
        bool Enabled { get; }

        void Transform(AControl element, double deltaTime);
    }

}
