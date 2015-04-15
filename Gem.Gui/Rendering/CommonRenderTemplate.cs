using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Gui.Rendering
{
    public class RenderTemplate
    {
        private readonly RenderStyle common;
        private readonly RenderStyle focus;
        private readonly RenderStyle clicked;

        public RenderTemplate(RenderStyle common, RenderStyle focus, RenderStyle clicked)
        {
            this.common = common;
            this.focus = focus;
            this.clicked = clicked;
        }

        RenderStyle Common { get { return common; } }

        RenderStyle Focus { get { return focus; } }

        RenderStyle Clicked { get { return clicked; } }
    }
}
