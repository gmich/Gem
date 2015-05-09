using Gem.Gui.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Gem.Gui.Rendering
{
    public interface ITextDrawable
    {
        void Render(SpriteBatch batch, IText text);
    }

}
