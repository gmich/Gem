using Gem.Engine.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Gem.Engine.ScreenSystem
{
    public interface IGame
    {
        Host Host { get; }

        void HandleInput(InputManager inputManager, GameTime gameTime);

        void FixedUpdate(GameTime gameTime);

        void Draw(SpriteBatch batch);
    }
}
