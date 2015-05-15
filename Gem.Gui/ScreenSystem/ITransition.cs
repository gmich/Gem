using Gem.Gui.Animations;
using Gem.Gui.Rendering;
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

        void Update(AnimationContext context);
        void Draw(RenderTarget2D renderTarget, ScreenState state, SpriteBatch batch);
    }
}
