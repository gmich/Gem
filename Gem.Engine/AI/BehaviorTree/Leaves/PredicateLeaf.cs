using System;

namespace Gem.AI.BehaviorTree.Leaves
{
    public class PredicateLeaf<AIContext> : IBehaviorNode<AIContext>
    {
        private readonly Predicate<AIContext> behaviorTest;
        private BehaviorResult behaviorResult;

        public PredicateLeaf(Predicate<AIContext> behaviorTest)
        {
            this.behaviorTest = behaviorTest;
        }

        public BehaviorResult Behave(AIContext context)
        {
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
