using RetroGameEngine.Diagnostics;
using System;

namespace RetroGameEngine.Animations.Events
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
