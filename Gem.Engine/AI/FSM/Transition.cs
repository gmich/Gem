using Gem.Infrastructure.Assertions;
using System;

namespace Gem.Engine.AI.FSM
{
    internal class Transition<TContext>
    {
        private readonly Func<AState<TContext>> nextState;
        private readonly Func<AState<TContext>> currentState;
        private readonly Action<AState<TContext>> stateSetter;
        private readonly AState<TContext> validStateForTransition;

        public Transition(
            AState<TContext> validStateForTransition,
            Func<AState<TContext>> nextState,
            Func<AState<TContext>> currentState,
            Action<AState<TContext>> stateSetter)
        {
            Argument.Require.NotNull(() => validStateForTransition);
            Argument.Require.NotNull(() => nextState);
            Argument.Require.NotNull(() => currentState);
            Argument.Require.NotNull(() => stateSetter);

            this.validStateForTransition = validStateForTransition;
            this.nextState = nextState;
            this.currentState = currentState;
            this.stateSetter = stateSetter;
        }

        public bool MakeTransition()
        {
            var current = currentState();
            if (current == validStateForTransition)
            {
                var newState = nextState();
                newState.OnEnter?.Invoke(newState, EventArgs.Empty);
                stateSetter(newState);
                current.OnExit?.Invoke(current, EventArgs.Empty);
                return true;
            }
            return false;
        }
    }
}
