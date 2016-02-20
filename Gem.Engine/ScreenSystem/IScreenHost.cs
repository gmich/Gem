using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Gem.Engine.Input;
using Gem.Engine.GameLoop;

namespace Gem.Engine.ScreenSystem
{
    public interface IScreenHost
    {
        event EventHandler<EventArgs> OnEntering;
        event EventHandler<EventArgs> OnExiting;

        event EventHandler<EventArgs> OnEnter;
        event EventHandler<EventArgs> OnExit;

        ScreenState ScreenState { get; }

        ITransition Transition { get; set; }

        void EnterScreen();

        void ExitScreen();

        void Initialize();

        void HandleInput(InputManager inputManager, ITimeline time);

        void Update(ITimeline time);

        void Draw(SpriteBatch batch);

    }

}
