using System;
using System.Collections.Generic;

namespace Gem.AI.BehaviorTree.Decorators
{
    public class Inverter<AIContext> : IDecorator<AIContext>
    {
        private readonly IBehaviorNode<AIContext> decoratedNode;
        private BehaviorResult behaviorResult;
        public event EventHandler OnBehaved;

        public Inverter(IBehaviorNode<AIContext> decoratedNode)
        {
            this.decoratedNode = decoratedNode;
        }

        public IEnumerable<IBehaviorNode<AIContext>> SubNodes
        { get { yield return decoratedNode; } }

        public string Name { get; set; } = string.Empty;
        public BehaviorResult Behave(AIContext context)
        {
            if (behaviorResult != BehaviorResult.Running)
            {
                return InvokeAndReturn();
            }

            switch (decoratedNode.Behave(context))
            {
                case BehaviorResult.Success:
                    behaviorResult = BehaviorResult.Failure;
                    break;
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
