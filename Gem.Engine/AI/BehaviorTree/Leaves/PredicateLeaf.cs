using System;
using System.Collections.Generic;

namespace Gem.AI.BehaviorTree.Leaves
{
    public class PredicateLeaf<AIContext> : IBehaviorNode<AIContext>
    {
        private readonly Predicate<AIContext> behaviorTest;
        private BehaviorResult behaviorResult;
        public event EventHandler OnBehaved;

        public PredicateLeaf(Predicate<AIContext> behaviorTest)
        {
            this.behaviorTest = behaviorTest;
        }

        public IEnumerable<IBehaviorNode<AIContext>> SubNodes
        { get { yield break; } }

        public string Name { get; set; } = string.Empty;

        public BehaviorResult Behave(AIContext context)
        {
            OnBehaved?.Invoke(this, new BehaviorInvokationEventArgs());
            if (behaviorResult != BehaviorResult.Running)
            {
                return behaviorResult;
            }

            var behaviorTestResult = behaviorTest(context);
            behaviorResult = behaviorTestResult ?
                BehaviorResult.Success : BehaviorResult.Failure;

            return behaviorResult;
        }
    }
}
