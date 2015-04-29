using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Gem.Gui.Rendering
{
    public class GuiSprite
    {
        private readonly Texture2D texture;
        private readonly Rectangle? sourceRectangle;

        public GuiSprite(Texture2D texture, Rectangle? sourceRectangle = null)
        {
            this.texture = texture;
            this.sourceRectangle = sourceRectangle;
        }

        public Texture2D Texture { get { return texture; } }

        public Rectangle? SourceRectangle { get { return sourceRectangle; } }
    }
}
