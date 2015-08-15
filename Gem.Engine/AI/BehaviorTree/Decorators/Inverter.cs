using System;
using System.Collections.Generic;

namespace Gem.AI.BehaviorTree.Decorators
{
    public class Inverter<AIContext> : IBehaviorNode<AIContext>
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

        public BehaviorResult Behave(AIContext context)
        {
            OnBehaved?.Invoke(this, new BehaviorInvokationEventArgs());
            if (behaviorResult != BehaviorResult.Running)
            {
                return behaviorResult;
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
            return behaviorResult;
        }
    }
}
