using System;
using System.Collections.Generic;

namespace Gem.AI.BehaviorTree.Leaves
{
    public class Question<AIContext> : ILeaf<AIContext>
    {
        private readonly Predicate<AIContext> behaviorTest;
        private BehaviorResult behaviorResult;
        public event EventHandler OnBehaved;

        public Question(Predicate<AIContext> behaviorTest)
        {
            behaviorResult = BehaviorResult.Running;
            this.behaviorTest = behaviorTest;
        }

        public IEnumerable<IBehaviorNode<AIContext>> SubNodes
        { get { yield break; } }

        public string Name { get; set; } = string.Empty;

        public BehaviorResult Behave(AIContext context)
        {
            var behaviorTestResult = behaviorTest(context);
            behaviorResult = behaviorTestResult ?
                BehaviorResult.Success : BehaviorResult.Failure;

            return InvokeAndReturn();
        }

        private BehaviorResult InvokeAndReturn()
        {
            OnBehaved?.Invoke(this, new BehaviorInvokationEventArgs(behaviorResult));
            return behaviorResult;
        }
    }
}
