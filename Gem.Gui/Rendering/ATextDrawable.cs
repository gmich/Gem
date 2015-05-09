using Gem.Gui.Controls;
using Gem.Gui.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Gem.Gui.Rendering
{    
    public abstract class ATextDrawable
    {
        protected readonly SpriteBatch batch;

        public ATextDrawable(SpriteBatch batch)
        {
            this.batch = batch;
        }

        public abstract void Render(IText text);
    }

}
