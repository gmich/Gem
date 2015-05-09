using Gem.Gui.Controls;
using Microsoft.Xna.Framework.Graphics;

namespace Gem.Gui.Rendering
{
    public interface IControlDrawable
    {
        void Render(SpriteBatch batch, AControl component);
    }
}
