using System;

namespace Gem.AI.FiniteStateMachine
{
    
    public class StateTransition<TStateContext>
    {
        private readonly Func<State<TStateContext>> stateGetter;
        private readonly Predicate<TStateContext> rule;
        public string Name { get; set; } = string.Empty;

        internal StateTransition(Func<State<TStateContext>> stateGetter, Predicate<TStateContext> rule)
        {
            this.stateGetter = stateGetter;
            this.rule = rule;
        }

        public StateTransition<TStateContext> Named(string name)
        {
            Name = name;
            return this;
        }

        internal State<TStateContext> Destination
        {  get { return stateGetter(); } }

        public bool Check(TStateContext context, State<TStateContext> currentState, Action<State<TStateContext>> stateSetter)
        {
            if (rule(context))
            {
                currentState.ExitState();
                var newState = stateGetter();
                stateSetter(newState);
                newState.EnterState();
                return true;
            }
            return false;
        }
    }

    public class Transition<TStateContext>
    {
        private readonly Func<State<TStateContext>> stateGetter;

        private Transition(Func<State<TStateContext>> stateGetter)
        {
            this.stateGetter = stateGetter;
        }

        public static Transition<TStateContext> To(
            Func<State<TStateContext>> stateGetter)
        {
            return new Transition<TStateContext>(stateGetter);
        }

        public StateTransition<TStateContext> When(
            Predicate<TStateContext> rule)
        {
            return new StateTransition<TStateContext>(stateGetter, rule);
        }
    }
}
