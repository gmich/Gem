using System.Threading.Tasks;

namespace Gem.Engine.AI.FSM
{
    public class StateMachine<TContext>
    {
        public AState<TContext> Current { get; internal set; }

        public void Update(TContext context)
        {
            Current.Update(context);
        }
    }
}
