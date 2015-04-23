using Gem.Gui.Configuration;
using Gem.Gui.Layout;
using Gem.Gui.Rendering;
using Gem.Gui.Transformation;

namespace Gem.Gui.Elements
{
    public interface IGuiElement
    {
        int Order { get; }

        RenderStyle RenderStyle { get; }

        GuiSprite Sprite { get;  }

        LayoutStyle LayoutStyle { get; }

        Options Options { get; set; }

        Region Region { get; }

        IGuiElement Parent { get; }

        void Update(double deltaTime);

        void Draw(ADrawManager manager);
    }
}