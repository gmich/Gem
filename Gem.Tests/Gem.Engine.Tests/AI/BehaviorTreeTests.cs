using Gem.AI.BehaviorTree;
using Gem.AI.BehaviorTree.Composites;
using Gem.AI.BehaviorTree.Decorators;
using Gem.AI.BehaviorTree.Leaves;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Engine.Tests.AI
{
    [TestClass]
    public class BehaviorTreeTests
    {
        [TestMethod]
        public void WalkAndUnlockDoorBehaviorTest()
        {
            using (var sw = File.CreateText("WalkAndUnlockDoorBehaviorTest.info"))
            {
                int step = 0;

                var walk = new ActionLeaf<AIContext>(
                context => CheckTarget(step = context.InitialStep, context.Target, sw.WriteLine),
                context => CheckTarget(++step, context.Target, sw.WriteLine))
                .TraceAs("Walk Action", sw.WriteLine);

                var unlockDoor = new ActionLeaf<AIContext>(
                context => context.CanUnlock ? BehaviorResult.Success : BehaviorResult.Failure)
                .TraceAs("Unlock Door Action", sw.WriteLine);

                var breakDoor = new ActionLeaf<AIContext>(
               context => BehaviorResult.Success)
               .TraceAs("Break Door Action", sw.WriteLine);

                var closeDoor = new ActionLeaf<AIContext>(
                context => BehaviorResult.Failure)
                .TraceAs("Close Door Action", sw.WriteLine);

                var openDoor = new Selector<AIContext>(new[] { unlockDoor, breakDoor.TraceAs("BreakDoor") })
                .TraceAs("Open Door Composite", sw.WriteLine);

                var goToRoom = new Sequence<AIContext>(new[] { walk, openDoor, DecorateFor.AlwaysSucceeding(closeDoor) })
                .TraceAs("Go To Room Composite", sw.WriteLine);

                var aiContext = new AIContext();

                for (int tick = 0; tick < 50; tick++)
                {
                    goToRoom.Behave(aiContext);
                }
            }
        }

        internal class AIContext
        {
            public int InitialStep { get; } = 1;
            public int Target { get; set; } = 10;
            public bool CanUnlock { get; } = true;
        }

        private BehaviorResult CheckTarget(int currentStep, int target, Action<string> debugInfo)
        {
            if (currentStep >= target)
            {
                return BehaviorResult.Success;
            }
            debugInfo(currentStep.ToString());
            return BehaviorResult.Running;
        }
    }
}
