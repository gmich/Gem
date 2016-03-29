using Gem.Infrastructure.Assertions;
using System;
using System.Collections.Generic;

namespace Gem.Engine.AI.FSM
{
    public class FSMBuilder<TContext>
    {
        private readonly Dictionary<string, AState<TContext>> states = new Dictionary<string, AState<TContext>>();
        private Action<AState<TContext>> stateSetter;
        private Func<AState<TContext>> stateGetter;
        private readonly StateMachine<TContext> stateMachine = new StateMachine<TContext>();

        public FSMBuilder()
        {
            stateSetter = state => stateMachine.Current = state;
            stateGetter = () => stateMachine.Current;
        }
        public FSMBuilder<TContext> AddState(string stateId, AState<TContext> state)
        {
            Argument.Require.NotNullOrEmpty(() => stateId);
            Argument.Require.NotNull(() => state);

            if (states.ContainsKey(stateId))
            {
                throw new FSMException($"A state with id: {stateId} has already been added.");
            }
            states.Add(stateId, state);
            return this;
        }

        public FSMBuilder<TContext> AddState(string stateId, Action<TContext> update)
        {
            Argument.Require.NotNullOrEmpty(() => stateId);
            Argument.Require.NotNull(() => update);

            if (states.ContainsKey(stateId))
            {
                throw new FSMException($"A state with id: {stateId} has already been added.");
            }
            states.Add(stateId, new State<TContext>(update));

            return this;
        }

        private AState<TContext> GetState(string stateId)
        {
            Argument.Require.NotNullOrEmpty(() => stateId);

            if (!states.ContainsKey(stateId))
            {
                throw new FSMException($"Cannot find state with id: {stateId}.");
            }
            return states[stateId];
        }

        public EventBuilder From(string current)
        {
            Argument.Require.NotNullOrEmpty(() => current);

            return new EventBuilder(GetState(current), GetState, stateGetter, stateSetter);
        }

        public StateMachine<TContext> Build(string initialState)
        {
            Argument.Require.NotNullOrEmpty(() => initialState);
            stateMachine.Current = GetState(initialState);

            return stateMachine;
        }

        public class EventBuilder
        {
            private readonly AState<TContext> current;
            private readonly Func<string, AState<TContext>> stateGetter;
            private readonly Func<AState<TContext>> currentStateGetter;
            private readonly Action<AState<TContext>> stateSetter;

            public EventBuilder(
                AState<TContext> current,
                Func<string, AState<TContext>> stateGetter,
                Func<AState<TContext>> currentStateGetter,
                Action<AState<TContext>> stateSetter)
            {
                this.stateSetter = stateSetter;
                this.stateGetter = stateGetter;
                this.currentStateGetter = currentStateGetter;
                this.current = current;
            }

            public IEvent To(string target)
            {
                var targetState = stateGetter(target);
                current.AddAdjacent(targetState);
                targetState.AddAdjacent(current);
                return new FsmEvent<TContext>(
                    new Transition<TContext>(
                        current, 
                        () => targetState,
                        currentStateGetter, 
                        stateSetter
                        )
                     );
            }
        }

    }
}
