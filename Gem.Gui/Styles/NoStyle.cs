using Gem.Gui.Rendering;
using Microsoft.Xna.Framework.Graphics;

namespace Gem.Gui.Styles
{
    public class NoStyle : ARenderStyle
    {
        public override void Render(IRenderable renderable, SpriteBatch batch)
        {
            return;
        }        
    }
}
