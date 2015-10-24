using Gem.Engine.AI.BehaviorTree;
using Gem.Engine.AI.BehaviorTree.Decorators;

namespace Gem.Engine.BehaviorTreeVisualization.Behaviors
{
    internal class FluentBehavior
    {
        public IBehaviorNode<BehaviorContext> Behavior { get; }

        public FluentBehavior()
        {
            var behaviorBuilder = new BehaviorTreeBuilder<BehaviorContext>();
            Behavior =
                    behaviorBuilder
                    .Selector
                       .Behavior(context => BehaviorResult.Success)
                       .Sequence
                            .Behavior(context => BehaviorResult.Success)
                       .End
                       .Sequence
                            .Decorate(DecorateFor.AlwaysSucceeding)
                            .Behavior(context => BehaviorResult.Success)
                            .Sequence
                               .Behavior(context => BehaviorResult.Success)
                            .End
                           .Question(context => 1 == 1)
                       .End
                    .End
                    .Tree;
        }
    }
}
