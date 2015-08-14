namespace Gem.AI.BehaviorTree
{
    public interface IBehaviorNode<AIContext>
    {
        BehaviorResult Behave(AIContext context);
    }
}
