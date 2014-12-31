using Microsoft.Xna.Framework;
using Gem.Animations.Events;
using System;

namespace Gem.Animations
{
    public interface IAnimation : IUpdateable,IDrawable
    {
        IAnimationEvent<IAnimation, EventArgs> Events { get; }

        void Stop(bool force = false);

        void Begin();
    }
}
