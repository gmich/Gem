using Gem.Infrastructure.Assertions;
using System;

namespace Gem.Engine.AI.FSM
{
    internal class FsmEvent<TContext> : IEvent
    {
        private readonly Transition<TContext> transition;
        public FsmEvent(Transition<TContext> transition)
        {
            Argument.Require.NotNull(() => transition);
            this.transition = transition;
        }

        public EventHandler OnTransition { get; }

        public void Raise()
        {
            if (transition.MakeTransition())
            {
                OnTransition?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
