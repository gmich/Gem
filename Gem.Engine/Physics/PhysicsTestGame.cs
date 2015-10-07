using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Gem.Engine.Containers;
using Gem.Engine.Input;
using Gem.Engine.ScreenSystem;

namespace Gem.Engine.Physics
{
    public class PhysicsTestGame : PhysicsHost
    {
        public PhysicsTestGame(ITransition transition, GraphicsDevice device, ContentContainer container)
            : base(transition, device, container)
        { }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void HandleInput(InputManager inputManager, GameTime gameTime)
        {
            base.HandleInput(inputManager, gameTime);
        }

        public override void FixedUpdate(GameTime gameTime)
        {
            base.FixedUpdate(gameTime);
        }

        public override void Draw(SpriteBatch batch)
        {
            base.Draw(batch);
        }

    }
}
