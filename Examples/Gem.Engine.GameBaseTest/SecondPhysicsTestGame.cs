using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Gem.Engine.Containers;
using Gem.Engine.Input;
using Gem.Engine.ScreenSystem;
using Gem.Engine.Physics;
using System;

namespace Gem.Engine.GameBaseTest
{
    public class SecondPhysicsTestGame : IPhysicsGame
    {
        public const string HostName = "second";
        public PhysicsHost Host
        {
            get;
        }
        
        public SecondPhysicsTestGame(PhysicsHost host)
        {
            Host = host;
        }

        public void Draw(SpriteBatch batch)
        {
            Host.Device.Clear(Color.Green);
        }

        public void HandleInput(InputManager inputManager, GameTime gameTime)
        {

        }

        public void FixedUpdate(GameTime gameTime)
        {

        }
    }
}
