using Gem.Engine.ScreenSystem;
using Microsoft.Xna.Framework.Input;

namespace Gem.Engine.GameBaseTest
{
    internal class TestGame
    {
        public GameBase GameBase { get; }

        public TestGame()
        {
            GameBase = new GameBase(windowWidth: 640, windowHeight: 480);
            GameBase.IsMouseVisible = true;

            GameBase.WhenReady += (sender, args) =>
            {
                GameBase.AddHost(FirstPhysicsTestGame.HostName, host => new FirstPhysicsTestGame(host), TimedTransition.Zero);
                GameBase.AddHost(SecondPhysicsTestGame.HostName, host => new SecondPhysicsTestGame(host), TimedTransition.Zero);
                GameBase.AddHost(TestGameScreen.HostName, host => new TestGameScreen(host), TimedTransition.Zero);
                GameBase.Show(FirstPhysicsTestGame.HostName);
            };
            GameBase.OnUpdate += (sender, args) =>
            {
                if (GameBase.Input.Keyboard.IsKeyClicked(Keys.Z))
                {
                    GameBase.ShowOnly(FirstPhysicsTestGame.HostName);
                }
                if (GameBase.Input.Keyboard.IsKeyClicked(Keys.X))
                {
                    GameBase.ShowOnly(SecondPhysicsTestGame.HostName);
                }
                if (GameBase.Input.Keyboard.IsKeyClicked(Keys.V))
                {
                    GameBase.ShowOnly(TestGameScreen.HostName);
                }
                if (GameBase.Input.Keyboard.IsKeyClicked(Keys.F12))
                {
                    GameBase.Reporter.Message("test entry");
                }
            };
        }

        private void Swap(string first, string second)
        {
            if (GameBase.IsShowing(first))
            {
                GameBase.Swap(first, second);
            }
            if (GameBase.IsShowing(second))
            {
                GameBase.Swap(second, first);
            }
        }


    }
}
