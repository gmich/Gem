using Gem.Gui.Configuration;
using Gem.Gui.Layout;
using Gem.Gui.Rendering;
using Gem.Gui.Transformation;

namespace Gem.Gui.Elements
{
    public interface IGuiElement
    {
        int Order { get; }

        RenderTemplate RenderTemplate { get; }

        LayoutStyle LayoutStyle { get; }

        Options Options { get; set; }

        Region Region { get; }
        
        void AddTransformation(ITransformation transformation);

        IGuiElement Parent { get; set; }

        void Update(double deltaTime);

        void Draw(IDrawManager manager);
    }
}