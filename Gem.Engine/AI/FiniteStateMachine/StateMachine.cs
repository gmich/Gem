using System;

namespace Gem.AI.FiniteStateMachine
{
    public class StateMachine<TStateContext>
    {
        public State<TStateContext> ActiveState { get; private set; }

        internal StateMachine(State<TStateContext> initialState)
        {
            if (initialState == null)
            {
                throw new ArgumentNullException("state");
            }
            ActiveState = initialState;
        }

        public void Graph()
        {

        }

        private void SetState(State<TStateContext> newState)
        {
            ActiveState = newState;
        }

        public void Operate(TStateContext context)
        {
            ActiveState.Update(context, SetState);
        }
    }

    public static class StateMachine
    {
        public static StateMachine<TStateContext> Create<TStateContext>(State<TStateContext> initialState)
        {
            return new StateMachine<TStateContext>(initialState);
        }
    }
}
