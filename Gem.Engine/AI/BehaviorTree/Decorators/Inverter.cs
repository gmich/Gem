namespace Gem.AI.BehaviorTree.Decorators
{
    public class Inverter<AIContext> : IBehaviorNode<AIContext>
    {
        private readonly IBehaviorNode<AIContext> decoratedNode;
        private BehaviorResult behaviorResult;

        public Inverter(IBehaviorNode<AIContext> decoratedNode)
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
