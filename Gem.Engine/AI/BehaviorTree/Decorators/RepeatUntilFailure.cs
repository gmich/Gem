using System;
using System.Collections.Generic;

namespace Gem.Engine.AI.BehaviorTree.Decorators
{
    public class RepeatUntilFailure<AIContext> : IDecorator<AIContext>
    {
        private readonly Func<IBehaviorNode<AIContext>> repeatedDecoratedNode;
        private IBehaviorNode<AIContext> decoratedNode;
        private BehaviorResult behaviorResult;
        public event EventHandler OnBehaved;

        public RepeatUntilFailure(Func<IBehaviorNode<AIContext>> repeatedOnSucess)
        {
            behaviorResult = BehaviorResult.Running;
            repeatedDecoratedNode = repeatedOnSucess;
            decoratedNode = repeatedOnSucess();
        }

        public string Name { get; set; } = string.Empty;
        public IEnumerable<IBehaviorNode<AIContext>> SubNodes
        { get { yield return decoratedNode; } }

        public BehaviorResult Behave(AIContext context)
        {         
            if(behaviorResult == BehaviorResult.Failure)
            {
                InvokeAndReturn();
            }
            behaviorResult = decoratedNode.Behave(context);

            if (behaviorResult == BehaviorResult.Success)
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
