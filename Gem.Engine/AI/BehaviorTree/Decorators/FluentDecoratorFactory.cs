using System;

namespace Gem.AI.BehaviorTree.Decorators
{
    public sealed class DecorateFor
    {
        public static IBehaviorNode<AIContext> AlwaysSucceeding<AIContext>(IBehaviorNode<AIContext> decoratedNode)
        {
            return new Succeeder<AIContext>(decoratedNode);
        }

        public static IBehaviorNode<AIContext> RepeatingUntilFailure<AIContext>(Func<IBehaviorNode<AIContext>> decoratedNodeFactory)
        {
            return new RepeatUntilFailure<AIContext>(decoratedNodeFactory);
        }
    }
    
}
