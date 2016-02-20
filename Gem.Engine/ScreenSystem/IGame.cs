using Gem.Engine.GameLoop;
using Gem.Engine.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Gem.Engine.ScreenSystem
{
    public interface IGame
    {
        Host Host { get; }

        void HandleInput(InputManager inputManager, ITimeline time);

        void FixedUpdate(ITimeline time);

        void Draw(SpriteBatch batch);
    }
}
