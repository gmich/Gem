using GameEngine2D.Diagnostics;
using System;

namespace GameEngine2D.Animations.Events
{

    public interface IAnimationEvent<Tsender, Targs> : IEventProvider<Targs>
        where Targs : EventArgs
        where Tsender : IAnimation
    {

        void RaiseStartEvent(Tsender sender, Targs args);

        void RaiseProgressEvent(Tsender sender, Targs args);

        void RaiseEndEvent(Tsender sender, Targs args);

    }
}
