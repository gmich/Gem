using Gem.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gem.Engine.AI.FiniteStateMachine
{
    public class State<TStateContext>
    {
        private readonly List<StateTransition<TStateContext>> transitions
            = new List<StateTransition<TStateContext>>();

        private readonly Action<TStateContext> stateBehavior;
        public event EventHandler onEnter;
        public event EventHandler onExit;

        public State(Action<TStateContext> stateBehavior)
        {
            this.stateBehavior = stateBehavior;
        }

        public string Name { get; set; } = string.Empty;

        public State<TStateContext> Named(string name)
        {
            Name = name;
            return this;
        }
         
        public IDisposable AddTransition(StateTransition<TStateContext> transition)
        {
            transitions.Add(transition);
            return Disposable.Create(transitions, transition);
        }

        public bool RemoveTransition(StateTransition<TStateContext> transition)
        {
            return transitions.Remove(transition);
        }
        internal IEnumerable<State<TStateContext>> ConnectedStates()
        {
            return transitions.Select(x => x.Destination);
        }

        internal void EnterState()
        {
            onEnter?.Invoke(this, EventArgs.Empty);
        }

        internal void ExitState()
        {
            onExit?.Invoke(this, EventArgs.Empty);
        }

        internal void Update(TStateContext context, Action<State<TStateContext>> setState)
        {
            if (transitions.Any(transition => transition.Check(context, this, setState)))
                return;

            stateBehavior(context);
        }
    }

}
