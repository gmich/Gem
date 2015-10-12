using Gem.Engine.Controls.Structure;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Engine.Controls.Rendering
{
    public interface IRenderer
    {
        void Update(double timeDelta);
        void Render(IGuiElement view, Rectangle parentBoundaries, RenderContext context);
        void Render(Rectangle parentBoundaries, List<Rectangle> siblingBoundaries, RenderContext context);
    }
}
