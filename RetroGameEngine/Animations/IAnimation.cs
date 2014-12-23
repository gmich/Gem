using Microsoft.Xna.Framework;
using RetroGameEngine.Animations.Events;
using System;

namespace RetroGameEngine.Animations
{
    public interface IAnimation : IUpdateable,IDrawable
    {
        IAnimationEvent<IAnimation, EventArgs> Events { get; }

        void Stop(bool force = false);

        void Begin();
    }
}
