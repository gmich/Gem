using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Gem.Gui.ScreenSystem
{
    public interface ITransition
    {
        event EventHandler<float> ProgressChanged;
        event EventHandler<TransitionDirection> TransitionStarted;
        event EventHandler<TransitionDirection> TransitionFinished;

        float Progress { get; }

        void Start(TransitionDirection direction);

        void Update(GameTime gameTime);
        void Draw(RenderTarget2D target, ScreenState state, SpriteBatch batch);
    }
}
