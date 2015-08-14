using System.Collections.Generic;
using System.Linq;

namespace Gem.AI.BehaviorTree.Composites
{
    /// <summary>
    /// Iterates BehaviorNodes in sequence and terminates upon failure. Behaves like logical AND
    /// </summary>
    /// <typeparam name="AIContext">The context to act upon</typeparam>
    public class Sequence<AIContext> : IBehaviorNode<AIContext>
    {
        private readonly Stack<IBehaviorNode<AIContext>> pendingNodes;
        private BehaviorResult behaviorResult;

        public Sequence(IBehaviorNode<AIContext>[] nodes)
        {
            pendingNodes = new Stack<IBehaviorNode<AIContext>>(nodes.Reverse());
        }

        private bool HasProcessedAllNodes
        {
            get { return pendingNodes.Count == 0; }
        }

        public BehaviorResult Behave(AIContext context)
        {
            if (HasProcessedAllNodes)
            {
                return behaviorResult;
            }

            var currentNode = pendingNodes.Pop();

            switch (behaviorResult = currentNode.Behave(context))
            {
                case BehaviorResult.Running:
                    //reevaluate the next iteration
                    pendingNodes.Push(currentNode);
                    break;
                case BehaviorResult.Failure:
                    //stop iterating nodes
                    pendingNodes.Clear();
                    break;
            }
            return BehaviorResult.Running;
        }
    }
}
