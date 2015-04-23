using Gem.Gui.Aggregators;
using Gem.Gui.Layout;
using Gem.Gui.Rendering;
using System;

namespace Gem.Gui.Elements
{

    public class DesktopElement<TEventArgs> : GuiElement<TEventArgs>
        where TEventArgs : EventArgs
    {

        public DesktopElement(RenderTemplate renderTemplate,
                              GuiSprite mouseCaptureSprite,
                              LayoutStyle layoutStyle,
                              Region region,
                              Func<IGuiElement, TEventArgs> eventArgsProvider,
                              int order = 0,
                              IGuiElement parent = null)
            : base(renderTemplate, layoutStyle, region, order, parent)
        {
            aggregator = new MouseControlAggregator<TEventArgs>(eventArgsProvider,
                                                                this.control,
                                                                new Input.DesktopInputHelper());
            renderTemplate.AddGuiSprite("MouseCapture", mouseCaptureSprite);
            this.Events.GotMouseCapture += (sender, args) => this.Sprite = renderTemplate["MouseCapture"];
            this.Events.LostMouseCapture += (sender, args) => this.Sprite = renderTemplate.Common;
        }
        
    }
}
