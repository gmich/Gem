using Gem.Gui.Controls;
using Microsoft.Xna.Framework.Graphics;

namespace Gem.Gui.Styles
{
    public interface IRenderStyle
    {
        void Focus(AControl styeControl);
        void Default(AControl styeControl);
        void Hover(AControl styeControl);
        void Clicked(AControl styeControl);

        void Render(SpriteBatch batch);
    }
}
