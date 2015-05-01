using Gem.Gui.Elements;
using Gem.Gui.Layout;
using Gem.Gui.Rendering;
using System;

namespace Gem.Gui.Builders
{

    interface IComponentBuilder 
    {
        RenderTemplateBuilder<IComponentBuilder> BuildRenderTemplate();
        IComponentBuilder BuildRegion(Func<Region> region);
        IComponentBuilder SetAlignment(IHorizontalAlignable horizontal, IVerticalAlignable vertical);
    }

    interface RenderTemplateBuilder<IBuilder>
        where IBuilder : IComponentBuilder
    {
        RenderTemplateBuilder<IBuilder> BuildRenderStyle(Func<RenderParameters> style);
        RenderTemplateBuilder<IBuilder> AddGuiSprite(string id, GuiSprite sprite);
        IBuilder End();
    }
    
}
