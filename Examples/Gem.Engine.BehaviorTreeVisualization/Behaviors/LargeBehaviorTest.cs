using System;
using Gem.AI.BehaviorTree;
using Gem.AI.BehaviorTree.Composites;
using Gem.AI.BehaviorTree.Decorators;
using Gem.AI.BehaviorTree.Leaves;

namespace Gem.Engine.BehaviorTreeVisualization.Behaviors
{
    internal class LargeBehavior
    {
        IBehaviorNode<AIContext> Behavior { get; }

        public LargeBehavior()
        {
            int step = 0;

            var walk = new Behavior<AIContext>(
            context => CheckTarget(step = context.InitialStep, context.Target),
            context => CheckTarget(++step, context.Target));
            walk.Name = "walk";

            var unlockDoor = new Behavior<AIContext>(
            context => context.CanUnlock ? BehaviorResult.Success : BehaviorResult.Failure);
            unlockDoor.Name = "unlock door";

            var breakDoor = new Behavior<AIContext>(
           context => BehaviorResult.Success);
            breakDoor.Name = "break door";

            var closeDoor = new Behavior<AIContext>(
            context => BehaviorResult.Failure);
            closeDoor.Name = "close door";

            var checkIfDoorIsCLosed = new Question<AIContext>(
            context => false);
            checkIfDoorIsCLosed.Name = "is door closed?";

            var lockDoor = new Behavior<AIContext>(
            context => BehaviorResult.Success);
            lockDoor.Name = "lock door";

            var openDoor = new Selector<AIContext>(new[] { unlockDoor, breakDoor });
            openDoor.Name = "open door";

            Behavior = new Sequence<AIContext>(new[] { walk, openDoor, DecorateFor.AlwaysSucceeding(closeDoor), checkIfDoorIsCLosed, lockDoor });
            Behavior.Name = "go to room";

        }

        internal class AIContext
        {
            public int InitialStep { get; } = 5;
            public int Target { get; set; } = 10;
            public bool CanUnlock { get; } = false;
        }

        private BehaviorResult CheckTarget(int currentStep, int target)
        {
            if (currentStep >= target)
            {
                return BehaviorResult.Success;
            }
            return BehaviorResult.Running;
        }

    }
}
