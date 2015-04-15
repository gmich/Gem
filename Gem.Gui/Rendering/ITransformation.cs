using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Gui.Rendering
{
    interface ITransformation
    {
        void TransformLocation(Func<Region> transformation);

        void TransformRenderStyle(Func<RenderStyle> transformation);
                
    }
}
