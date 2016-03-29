using System;
using System.Collections.Generic;

namespace Gem.Engine.AI.FSM
{

    public abstract class AState<TContext>
    {
        private readonly IList<AState<TContext>> adjacentStates = new List<AState<TContext>>();

        public EventHandler OnEnter { get; }
        public EventHandler OnExit { get; }

        public IEnumerable<AState<TContext>> Adjacent { get; }

        internal void AddAdjacent(AState<TContext> state)
        {
            adjacentStates.Add(state);
        }

        public abstract void Update(TContext context);
    }
}
