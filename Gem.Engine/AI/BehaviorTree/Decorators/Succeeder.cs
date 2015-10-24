using System;
using System.Collections.Generic;

namespace Gem.Engine.AI.BehaviorTree.Decorators
{
    public class Succeeder<AIContext> : IDecorator<AIContext>
    {
        private readonly IBehaviorNode<AIContext> decoratedNode;
        private BehaviorResult behaviorResult;
        public event EventHandler OnBehaved;

        public Succeeder(IBehaviorNode<AIContext> decoratedNode)
        {
            behaviorResult = BehaviorResult.Running;
            this.decoratedNode = decoratedNode;
        }

        public IEnumerable<IBehaviorNode<AIContext>> SubNodes
        { get { yield return decoratedNode; } }

        public string Name { get; set; } = "Decorator";

        public BehaviorResult Behave(AIContext context)
        {
            if (behaviorResult != BehaviorResult.Running)
            {
                return InvokeAndReturn();
            }

            switch (decoratedNode.Behave(context))
            {
                case BehaviorResult.Success:
                case BehaviorResult.Failure:
                    behaviorResult = BehaviorResult.Success;
                    break;
            }
            return InvokeAndReturn();
        }

        private BehaviorResult InvokeAndReturn()
        {
            OnBehaved?.Invoke(this, new BehaviorInvokationEventArgs(behaviorResult));
            return behaviorResult;
        }
    }
}
