using Gem.Gui.Configuration;
using Gem.Gui.Controls;
using Gem.Gui.Layout;
using Gem.Gui.Rendering;
using System;

namespace Gem.Gui.Elements
{
    interface IGuiElement 
    {
        RenderTemplate RenderTemplate { get; }

        LayoutStyle LayoutStyle { get;  }

        Options Options { get; set; }

        Region Region { get; }

        IGuiElement Parent { get; set; }

        CommonEventAggregator<ControlEventArgs> Events { get; set;  }

        void TransformLocation(Func<Region> transformation);

        void TransformRenderStyle(Func<RenderTemplate> transformation);
        
        void Update();

        void Draw(DrawManager manager);
    }
}
