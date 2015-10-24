using System;
using System.Collections.Generic;

namespace Gem.Engine.AI.BehaviorTree.Decorators
{
    public class RepeatUntilSuccess<AIContext> : IDecorator<AIContext>
    {
        private readonly Func<IBehaviorNode<AIContext>> repeatedDecoratedNode;
        private IBehaviorNode<AIContext> decoratedNode;
        private BehaviorResult behaviorResult;
        public event EventHandler OnBehaved;

        public RepeatUntilSuccess(Func<IBehaviorNode<AIContext>> repeatedOnFailure)
        {
            behaviorResult = BehaviorResult.Running;
            repeatedDecoratedNode = repeatedOnFailure;
            decoratedNode = repeatedOnFailure();
        }

        public string Name { get; set; } = string.Empty;
        public IEnumerable<IBehaviorNode<AIContext>> SubNodes
        { get { yield return decoratedNode; } }

        public BehaviorResult Behave(AIContext context)
        {         
            if(behaviorResult == BehaviorResult.Success)
            {
                InvokeAndReturn();
            }
            behaviorResult = decoratedNode.Behave(context);

            if (behaviorResult == BehaviorResult.Failure)
            {
                decoratedNode = repeatedDecoratedNode();
                behaviorResult = BehaviorResult.Running;
            }

            return InvokeAndReturn();
        }

        public BehaviorResult InvokeAndReturn()
        {
            OnBehaved?.Invoke(this, new BehaviorInvokationEventArgs(behaviorResult));
            return behaviorResult;
        }
    }
}
