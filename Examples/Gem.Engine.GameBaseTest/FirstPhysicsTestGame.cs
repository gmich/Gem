using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Gem.Engine.Physics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Gem.Engine.Input;
using Gem.Engine.GameLoop;

namespace Gem.Engine.GameBaseTest
{
    public class FirstPhysicsTestGame : IPhysicsGame
    {
        private Body rectangle;

        public const string HostName = "first";

        public PhysicsHost Host { get;  }

        public FirstPhysicsTestGame(PhysicsHost host)
        {
            Host = host;
            Host.World.Gravity = Vector2.Zero;

            rectangle = BodyFactory.CreateRectangle(Host.World, 5f, 5f, 1f);
            rectangle.BodyType = BodyType.Dynamic;

            Host.SetUserAgent(rectangle, 100f, 100f);
        }

        public void Draw(SpriteBatch batch)
        {
            Host.Device.Clear(Color.Beige);
        }

        public void HandleInput(InputManager inputManager, ITimeline time)
        {
        }

        public void FixedUpdate(ITimeline time)
        {
        }
    }
}
