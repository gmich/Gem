using Gem.AI.BehaviorTree;
using Gem.AI.BehaviorTree.Composites;
using Gem.AI.BehaviorTree.Decorators;
using Gem.AI.BehaviorTree.Leaves;

namespace Gem.Engine.BehaviorTreeVisualization.Behaviors
{
    internal class WalkBehavior
    {
        public IBehaviorNode<BehaviorContext> Behavior { get; }


        public WalkBehavior()
        {
            IBehaviorNode<BehaviorContext> checkNextTile
                = new PredicateLeaf<BehaviorContext>(
                 context => context.Level.NextTile is EmptyTile);
            checkNextTile.Name = "is tile safe?";

            IBehaviorNode<BehaviorContext> foundKey
                = new PredicateLeaf<BehaviorContext>(
                 context => context.Level.Map[context.Level.PlayerPosition + 1] is Key);
            foundKey.Name = "found key?";

            IBehaviorNode<BehaviorContext> foundDoor
                = new PredicateLeaf<BehaviorContext>(
                 context => context.Level.Map[context.Level.PlayerPosition + 1] is Door);
            foundDoor.Name = "found door?";

            IBehaviorNode<BehaviorContext> doIHaveKey
            = new PredicateLeaf<BehaviorContext>(context => context.Level.HaveKey);
            doIHaveKey.Name = "do i if have key?";

            IBehaviorNode<BehaviorContext> doIHaveEmptySpace
                = new PredicateLeaf<BehaviorContext>(context => !context.Level.HaveKey);
            doIHaveEmptySpace.Name = "can i pick it up?";

            IBehaviorNode<BehaviorContext> pickKeyUp
                = new ActionLeaf<BehaviorContext>(
                 context =>
                 {
                     context.Level.HaveKey = true;
                     context.Level.Map[context.Level.PlayerPosition+1]=new EmptyTile();
                     return BehaviorResult.Success;
                 });
            pickKeyUp.Name = "pick key up";

            IBehaviorNode<BehaviorContext> walk
                = new ActionLeaf<BehaviorContext>(context =>
                context.Level.MovePlayer(1) ?
                BehaviorResult.Success : BehaviorResult.Failure);
            walk.Name = "move 1 step";

            IBehaviorNode<BehaviorContext> skipKey
                = new ActionLeaf<BehaviorContext>(context =>
                context.Level.MovePlayer(1) ?
                BehaviorResult.Success : BehaviorResult.Failure);
                        skipKey.Name = "skip key";

            IBehaviorNode<BehaviorContext> unlockDoor
                = new ActionLeaf<BehaviorContext>(context =>
                {
                    context.Level.Map[context.Level.PlayerPosition + 1] = new EmptyTile();
                    context.Level.HaveKey = false;
                    return BehaviorResult.Success;
                });
            unlockDoor.Name = "unlock door";

            IBehaviorNode<BehaviorContext> goBack
                = new ActionLeaf<BehaviorContext>(context =>
                {
                    context.Level.MovePlayer(-1);
                    return BehaviorResult.Success;
                });
            goBack.Name = "go back";

            IBehaviorNode<BehaviorContext> isPreviousNodeKey
                = new PredicateLeaf<BehaviorContext>(
                 context => context.Level.Map[context.Level.PlayerPosition - 1] is Key);
            isPreviousNodeKey.Name = "found key?";

            IBehaviorNode<BehaviorContext> pickFoundKeyUp
            = new ActionLeaf<BehaviorContext>(
             context =>
             {
                 context.Level.HaveKey = true;
                 context.Level.Map[context.Level.PlayerPosition - 1] = new EmptyTile();
                 return BehaviorResult.Success;
             });
            pickFoundKeyUp.Name = "pick key up";

            var findKeySequence = new Sequence<BehaviorContext>(new[] { goBack, isPreviousNodeKey, pickFoundKeyUp });
            findKeySequence.Name = "find a key";

            var repeatUntiFoundAKey = DecorateFor.RepeatingUntilSuccess(() =>
            {
                findKeySequence.Reset();
                return findKeySequence;
            });
            repeatUntiFoundAKey.Name = "repeat until";


            var pickupKeySequence = new Sequence<BehaviorContext>(new[] { doIHaveEmptySpace, pickKeyUp });
            pickupKeySequence.Name = "try pick up";

            var whatToDoWithKey = new Selector<BehaviorContext>(new[] { pickupKeySequence, skipKey });
            whatToDoWithKey.Name = "how to handle key";

            var keySequence = new Sequence<BehaviorContext>(new[] { foundKey, whatToDoWithKey });
            keySequence.Name = "key obstacle";


            var unlockDoorSequence = new Sequence<BehaviorContext>(new[] { doIHaveKey, unlockDoor });
            unlockDoorSequence.Name = "unlockDoor";

            var goThroughDoorSelector = new Selector<BehaviorContext>(new[] { unlockDoorSequence, repeatUntiFoundAKey });
            goThroughDoorSelector.Name = "go through door";

            var doorSequence = new Sequence<BehaviorContext>(new[] { foundDoor, goThroughDoorSelector });
            doorSequence.Name = "door obstacle";

            var move = new Sequence<BehaviorContext>(new[] { checkNextTile, walk });
            move.Name = "try walk";

            var repeater = DecorateFor.RepeatingUntilFailure(() =>
           {
               move.Reset();
               return move;
           });
            repeater.Name = "move forward";

            Behavior = new Selector<BehaviorContext>(new[] { repeater, keySequence, doorSequence });
            Behavior.Name = "walk";
        }
    }
}
