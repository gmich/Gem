using Gem.Engine.ScreenSystem;
using Microsoft.Xna.Framework.Graphics;
using Gem.Engine.Input;
using Microsoft.Xna.Framework;
using Gem.Engine.GameLoop;

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

        public void FixedUpdate(ITimeline time)
        {
        }

        public void HandleInput(InputManager inputManager, ITimeline time)
        {
        }

        public void Initialize()
        {
        }
    }
}
