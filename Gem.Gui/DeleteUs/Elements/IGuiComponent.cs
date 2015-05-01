using Gem.Gui.Configuration;
using Gem.Gui.Controls;
using Gem.Gui.Alignment;
using Gem.Gui.Rendering;
using Gem.Gui.Transformation;
using System;
using Gem.Gui.Events;

namespace Gem.Gui.Elements
{
    public class ElementEventArgs : EventArgs
    {

    }

    public interface IGuiComponent
    {
        ViewEvents<ElementEventArgs> Events { get; }

        RenderParameters RenderStyle { get; }
               
        Options Options { get; set; }

        Region Region { get; }

        void Update(double deltaTime);

        void Draw(ABatchDrawable manager);
    }
}