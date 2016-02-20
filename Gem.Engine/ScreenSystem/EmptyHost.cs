using Microsoft.Xna.Framework.Graphics;
using Gem.Engine.Containers;
using Gem.Engine.Input;
using Microsoft.Xna.Framework;
using Gem.Engine.GameLoop;

namespace Gem.Engine.ScreenSystem
{
    
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

        public override void FixedUpdate(ITimeline time)
        {
            Game.FixedUpdate(time);
        }

        public override void HandleInput(InputManager inputManager, ITimeline time)
        {
            Game.HandleInput(inputManager, time);
        }

        public override void Initialize()
        {            
        }
    }
}
