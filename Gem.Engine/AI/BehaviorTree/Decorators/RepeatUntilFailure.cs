using System;
using System.Collections.Generic;

namespace Gem.AI.BehaviorTree.Decorators
{
    public class RepeatUntilFailure<AIContext> : IBehaviorNode<AIContext>
    {
        private readonly IBehaviorNode<AIContext> decoratedNode;
        private BehaviorResult behaviorResult;

        public event EventHandler OnBehaved;

        public RepeatUntilFailure(IBehaviorNode<AIContext> decoratedNode)
        {
            this.decoratedNode = decoratedNode;
        }

        public string Name { get; set; } = string.Empty;
        public IEnumerable<IBehaviorNode<AIContext>> SubNodes
        { get { yield return decoratedNode; } }

        public BehaviorResult Behave(AIContext context)
        {
            OnBehaved?.Invoke(this, new BehaviorInvokationEventArgs());
            if (behaviorResult == BehaviorResult.Failure)
            {
                return behaviorResult;
            }

            return (behaviorResult = decoratedNode.Behave(context));

        }
    }
}
