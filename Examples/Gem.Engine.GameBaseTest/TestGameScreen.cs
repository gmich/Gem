using Gem.Engine.ScreenSystem;
using Microsoft.Xna.Framework.Graphics;
using Gem.Engine.Input;
using Microsoft.Xna.Framework;

namespace Gem.Engine.GameBaseTest
{
    public class TestGameScreen : IGame
    {
        public const string HostName = "third";

        public Host Host
        {
            get;
        }

        public TestGameScreen(Host host)
        {
            Host = host;
        }

        public void Draw(SpriteBatch batch)
        {
            Host.Device.Clear(Color.Red);
        }

        public void FixedUpdate(GameTime gameTime)
        {
        }

        public void HandleInput(InputManager inputManager, GameTime gameTime)
        {
        }

        public void Initialize()
        {
        }
    }
}
