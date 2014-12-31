using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Gem.DrawingSystem
{
    public class SpriteFonts
    {
        public SpriteFont DetailsFont;
        public SpriteFont MenuSpriteFont;

        public SpriteFonts(ContentManager contentManager)
        {
            MenuSpriteFont = contentManager.Load<SpriteFont>("Fonts/menuFont");
            DetailsFont = contentManager.Load<SpriteFont>("Fonts/detailsFont");
        }
    }
}