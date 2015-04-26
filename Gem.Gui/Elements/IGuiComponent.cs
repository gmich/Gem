using Gem.Gui.Configuration;
using Gem.Gui.Controls;
using Gem.Gui.Layout;
using Gem.Gui.Rendering;
using Gem.Gui.Transformation;
using System;

namespace Gem.Gui.Elements
{
    public class ElementEventArgs : EventArgs
    {

    }

    public interface IGuiComponent
    {
        Control<ElementEventArgs> Events { get; }

        RenderStyle RenderStyle { get; }

        GuiSprite Sprite { get;  }

        Alignment Alignment { get; }

        Options Options { get; set; }

        Region Region { get; }

        IGuiComponent Parent { get; }

        void Update(double deltaTime);

        void Draw(ABatchDrawable manager);
    }
}