using Gem.Infrastructure.Assertions;
using System;

namespace Gem.Engine.AI.FSM
{
    internal class State<TContext> : AState<TContext>
    {
        private readonly Action<TContext> update;
        public State(Action<TContext> update)
        {
            Argument.Require.NotNull(() => update);
            this.update = update;
        }

        public override void Update(TContext context)
        {
            update(context);
        }
    }

}
