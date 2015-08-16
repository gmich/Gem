using Gem.AI.BehaviorTree;
using Gem.AI.BehaviorTree.Composites;
using Gem.AI.BehaviorTree.Decorators;
using Gem.AI.BehaviorTree.Leaves;

namespace Gem.Engine.BehaviorTreeVisualization.Behaviors
{
    internal class BehaviorChooser
    {
        public IBehaviorNode<BehaviorContext> Behavior { get; }

        public BehaviorChooser()
        {
            IBehaviorNode<BehaviorContext> checkNextTile
                = new PredicateLeaf<BehaviorContext>(
                 context => context.Level.Map[context.Level.PlayerPosition + 1] is EmptyTile);
            checkNextTile.Name = "found key?";

            IBehaviorNode<BehaviorContext> walk
                = new ActionLeaf<BehaviorContext>(context =>
                context.Level.MovePlayer(1) ?
                BehaviorResult.Success : BehaviorResult.Failure);
            walk.Name = "found door?";

            var move = new Sequence<BehaviorContext>(new[] { checkNextTile, walk });
            move.Name = "try walk";

            Behavior = DecorateFor.RepeatingUntilFailure(() =>
           {
               move.Reset();
               return move;
           });
        }
    }
}
