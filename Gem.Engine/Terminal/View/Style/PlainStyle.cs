using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Gem.Engine.GTerminal.View.Style
{
    public class PlainStyle : IRenderStyle
    {

        public void Render(IView view, Rectangle parentBoundaries, RenderContext context)
        {
            var currentRect = context.Batch.GraphicsDevice.ScissorRectangle;
            //context.Batch.GraphicsDevice.ScissorRectangle = view.Bounds;
            //context.Batch.Draw()
            context.Batch.GraphicsDevice.ScissorRectangle = currentRect;
        }

        public void Update(double timeDelta)
        {           
        }
    }
}
