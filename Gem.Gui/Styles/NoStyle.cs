using Gem.Gui.Controls;
using Microsoft.Xna.Framework.Graphics;

namespace Gem.Gui.Styles
{
    public class NoStyle : IRenderStyle
    {
       
        #region Style

        public void Focus(AControl styeControl)
        {
            return;
        }

        public void Default(AControl styeControl)
        {
            return;
        }

        public void Hover(AControl styeControl)
        {
            return;
        }

        public void Clicked(AControl styeControl)
        {
            return;
        }

        #endregion

        public void Render(SpriteBatch batch)
        {
            return;
        }
        
    }
}
