using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Gem.Console.Rendering
{
    public class Sprite : ITexture
    {
        private readonly Texture2D texture;
        private readonly Rectangle selection;

        public Sprite(Texture2D sprite)
        {
            this.texture = sprite;
            this.selection = new Rectangle(0, 0, sprite.Width, sprite.Height);
        }

        public Sprite(Texture2D sprite, Rectangle selection)
        {
            this.texture = sprite;
            this.selection = selection;
        }

        public Texture2D Texture
        {
            get { return texture; }
        }

        public Rectangle Frame
        {
            get { return selection; }
        }
        
        public void Update(GameTime gameTime)
        {
            return;
        }
    }
}
