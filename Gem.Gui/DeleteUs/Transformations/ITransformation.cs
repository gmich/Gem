using Gem.Gui.Elements;

namespace Gem.Gui.Transformation
{
    public interface ITransformation
    {
        bool Enabled { get; }

        void Transform(IGuiComponent element, double deltaTime);
    }

}
