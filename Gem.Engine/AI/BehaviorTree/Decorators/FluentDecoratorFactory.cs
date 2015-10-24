using System;

namespace Gem.Engine.AI.BehaviorTree.Decorators
{
    public delegate IBehaviorNode<AIContext> Decorator<AIContext>(IBehaviorNode<AIContext> decoratedNode);

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

        public static IBehaviorNode<AIContext> RepeatingUntilSuccess<AIContext>(Func<IBehaviorNode<AIContext>> decoratedNodeFactory)
        {
            return new RepeatUntilSuccess<AIContext>(decoratedNodeFactory);
        }
    }
    
}
