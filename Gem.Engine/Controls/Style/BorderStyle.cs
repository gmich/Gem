using Gem.Engine.Controls.Rendering;
using System;

namespace Gem.Engine.GTerminal.View
{

    public interface IBorderStyle
    {
        void Render(RenderContext context, BorderBox border);
    }

    public class BorderStyle : IBorderStyle
    {
        private readonly Action<RenderContext, BorderBox> renderAction;

        public BorderStyle(Action<RenderContext, BorderBox> renderAction)
        {
            this.renderAction = renderAction;
        }

        public static BorderStyle Solid
        {
            get
            {
                return new BorderStyle((context, batch) =>
                {

                });
            }
        }
            public void Render(RenderContext context, BorderBox border)
        {
            renderAction(context, border);
        }
    }
}
