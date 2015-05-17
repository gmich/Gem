using Microsoft.Xna.Framework.Graphics;

namespace Gem.Gui.Factories
{
    internal interface ITextureFactory
    {
        Texture2D GetTexture(TextureCreationRequest options);
    }
        
}
