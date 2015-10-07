using Microsoft.Xna.Framework.Graphics;
using Gem.Engine.Containers;
using Gem.Engine.Input;
using Microsoft.Xna.Framework;

namespace Gem.Engine.ScreenSystem
{
    public class EmptyHost : Host
    {
        private readonly IGame game;

        public EmptyHost(IGame game,
                          ITransition transition,
                          GraphicsDevice device,
                          ContentContainer container)
            : base(transition, device, container)
        {
            this.game = game;
            game.Host = this;
        }

        public override void Draw(SpriteBatch batch)
        {
            game.Draw(batch);
        }

        public override void FixedUpdate(GameTime gameTime)
        {
            game.FixedUpdate(gameTime);
        }

        public override void HandleInput(InputManager inputManager, GameTime gameTime)
        {
            game.HandleInput(inputManager, gameTime);
        }

        public override void Initialize()
        {
            game.Initialize();
        }
    }
}
