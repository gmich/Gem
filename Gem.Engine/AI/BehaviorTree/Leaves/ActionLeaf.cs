using System;

namespace Gem.AI.BehaviorTree.Leaves
{
    public class ActionLeaf<AIContext> : IBehaviorNode<AIContext>
    {
        public ActionLeaf(Func<AIContext, BehaviorResult> processedBehavior)
        {
            behaveDelegate = processedBehavior;
        }
        private Func<AIContext, BehaviorResult> behaveDelegate;

        public ActionLeaf(Func<AIContext, BehaviorResult> initialBehavior,
                          Func<AIContext, BehaviorResult> processedBehavior)
        {
            behaveDelegate = context =>
            {
                behaveDelegate = processedBehavior;
                return initialBehavior(context);
            };
        }
            
        public BehaviorResult Behave(AIContext context)
        {
            return behaveDelegate(context);
        }
    }
}
