using System;

namespace Gem.Engine.AI.FSM
{
    public interface IEvent
    {
        EventHandler OnTransition { get; }
        void Raise();
    }
}
