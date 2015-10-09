using Microsoft.Xna.Framework.Graphics;
using Gem.Engine.Containers;
using Gem.Engine.Input;
using Microsoft.Xna.Framework;
using NullGuard;

namespace Gem.Engine.ScreenSystem
{
    [NullGuard(ValidationFlags.AllPublicArguments)]
    public class EmptyHost : Host
    {
        public IGame Game
        {
            get; set;
        }

        public EmptyHost(ITransition transition,
                          GraphicsDevice device,
                          ContentContainer container)
            : base(transition, device, container)
        {    

        }

        public override void Draw(SpriteBatch batch)
        {
            Game.Draw(batch);
        }

        public override void FixedUpdate(GameTime gameTime)
        {
            Game.FixedUpdate(gameTime);
        }

        public override void HandleInput(InputManager inputManager, GameTime gameTime)
        {
            Game.HandleInput(inputManager, gameTime);
        }

        public override void Initialize()
        {            
        }
    }
}
