using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Gem.Engine.Console.Rendering.Animations
{
    public interface IEffect
    {
        void Draw(SpriteFont font, SpriteBatch batch, Vector2 location);
    }
}
