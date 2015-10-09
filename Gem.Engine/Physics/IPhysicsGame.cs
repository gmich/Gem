using Gem.Engine.Input;
using Gem.Engine.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Gem.Engine.Physics
{
    public interface IPhysicsGame
    {
        PhysicsHost Host { get; }

        void HandleInput(InputManager inputManager, GameTime gameTime);

        void FixedUpdate(GameTime gameTime);

        void Draw(SpriteBatch batch);
    }
}
