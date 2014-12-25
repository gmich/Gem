using Microsoft.Xna.Framework;
using GameEngine2D.Animations.Events;
using System;

namespace GameEngine2D.Animations
{
    public interface IAnimation : IUpdateable,IDrawable
    {
        IAnimationEvent<IAnimation, EventArgs> Events { get; }

        void Stop(bool force = false);

        void Begin();
    }
}
