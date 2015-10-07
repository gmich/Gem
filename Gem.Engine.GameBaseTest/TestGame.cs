using Gem.Engine.ScreenSystem;
using Microsoft.Xna.Framework.Input;

namespace Gem.Engine.GameBaseTest
{
    internal class TestGame
    {
        public GameBase GameBase { get; }

        public TestGame()
        {
            string firstHost = "first";
            string secondHost = "second";
            string thirdHost = "third";
            GameBase = new GameBase(640, 480);
            GameBase.IsMouseVisible = true;
            GameBase.OnReady += (sender, args) =>
            {
                GameBase.AddHost(firstHost, new FirstPhysicsTestGame(), TimedTransition.Default);
                GameBase.AddHost(secondHost, new SecondPhysicsTestGame(), TimedTransition.Default);
                GameBase.AddHost(thirdHost, new TestGameScreen(), TimedTransition.Default);
                GameBase.Show(firstHost);
            };
            GameBase.OnUpdate += (sender, args) =>
            {
                if (GameBase.Input.Keyboard.IsKeyClicked(Keys.Z))
                {
                    GameBase.ShowOnly(firstHost);
                }
                if (GameBase.Input.Keyboard.IsKeyClicked(Keys.X))
                {
                    GameBase.ShowOnly(secondHost);
                }
                if (GameBase.Input.Keyboard.IsKeyClicked(Keys.C))
                {
                    GameBase.ShowOnly(thirdHost);
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
