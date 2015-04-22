using Gem.Gui.Elements;

namespace Gem.Gui.Transformation
{
    public interface ITransformation
    {
        bool Enabled { get; }

        void Transform(IGuiElement element, double deltaTime);
    }

}
