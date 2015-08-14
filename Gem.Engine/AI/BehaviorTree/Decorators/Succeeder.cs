namespace Gem.AI.BehaviorTree.Decorators
{
    public class Succeeder<AIContext> : IBehaviorNode<AIContext>
    {
        private readonly IBehaviorNode<AIContext> decoratedNode;
        private BehaviorResult behaviorResult;

        public Succeeder(IBehaviorNode<AIContext> decoratedNode)
        {
            this.decoratedNode = decoratedNode;
        }
        
        public BehaviorResult Behave(AIContext context)
        {
            if (behaviorResult != BehaviorResult.Running)
            {
                return behaviorResult;
            }

            switch (decoratedNode.Behave(context))
            {
                case BehaviorResult.Success:
                case BehaviorResult.Failure:
                    behaviorResult = BehaviorResult.Success;
                    break;
            }
            return behaviorResult;
        }
    }
}
