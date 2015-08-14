namespace Gem.AI.BehaviorTree.Decorators
{
    public class RepeatUntilFailure<AIContext> : IBehaviorNode<AIContext>
    {
        private readonly IBehaviorNode<AIContext> decoratedNode;
        private BehaviorResult behaviorResult;

        public RepeatUntilFailure(IBehaviorNode<AIContext> decoratedNode)
        {
            this.decoratedNode = decoratedNode;
        }

        public BehaviorResult Behave(AIContext context)
        {
            if (behaviorResult == BehaviorResult.Failure)
            {
                return behaviorResult;
            }

            return (behaviorResult = decoratedNode.Behave(context));

        }
    }
}
