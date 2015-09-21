using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Gem.Engine.Containers
{
    public class ContentContainer
    {
        public ContentContainer(ContentManager content)
        {
            Textures = new AssetContainer<Texture2D>(content);
            Fonts = new AssetContainer<SpriteFont>(content);
        }
        public AssetContainer<Texture2D> Textures { get; }
        public AssetContainer<SpriteFont> Fonts { get; }
    }
}
