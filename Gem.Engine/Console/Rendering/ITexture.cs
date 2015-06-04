using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Gem.Console.Rendering
{
    public interface ITexture
    {
        Texture2D Texture { get;  }

        Rectangle Frame { get; }

        void Update(GameTime gameTime);
    }
}
