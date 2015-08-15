using System;
using System.Collections.Generic;
using System.Linq;

namespace Gem.AI.BehaviorTree.Composites
{
    /// <summary>
    /// Iterates BehaviorNodes in sequence and terminates upon failure. Behaves like the logical AND operator
    /// </summary>
    /// <typeparam name="AIContext">The context to act upon</typeparam>
    public class Selector<AIContext> : IBehaviorNode<AIContext>
    {
        private readonly Stack<IBehaviorNode<AIContext>> pendingNodes;
        private readonly IBehaviorNode<AIContext>[] nodes;
        private BehaviorResult behaviorResult;
        public event EventHandler OnBehaved;

        public Selector(IBehaviorNode<AIContext>[] nodes)
        {
            this.nodes = nodes;
            pendingNodes = new Stack<IBehaviorNode<AIContext>>(nodes.Reverse());
        }

        public string Name { get; set; } = string.Empty;

        public IEnumerable<IBehaviorNode<AIContext>> SubNodes
        { get { return nodes; } }

        private bool HasProcessedAllNodes => pendingNodes.Count == 0;

        public BehaviorResult Behave(AIContext context)
        {
            OnBehaved?.Invoke(this, new BehaviorInvokationEventArgs());
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
                case BehaviorResult.Success:
                    //stop iterating nodes
                    pendingNodes.Clear();
                    break;
            }
            return BehaviorResult.Running;
        }


    }
}
