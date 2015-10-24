using System;
using System.Collections.Generic;
using System.Linq;

namespace Gem.Engine.AI.BehaviorTree.Composites
{
    /// <summary>
    /// Sequence. Iterates BehaviorNodes terminates when a node succeeds. Behaves like the logical OR operator
    /// </summary>
    /// <typeparam name="AIContext">The context to act upon</typeparam>
    public class Sequence<AIContext> : IComposite<AIContext>
    {
        private readonly IBehaviorNode<AIContext>[] nodes;
        private Stack<IBehaviorNode<AIContext>> pendingNodes;
        private BehaviorResult behaviorResult;
        public event EventHandler OnBehaved;

        public Sequence(IBehaviorNode<AIContext>[] nodes)
        {
            this.nodes = nodes;
            pendingNodes = new Stack<IBehaviorNode<AIContext>>(nodes.Reverse());
        }

        public void Reset()
        {
            pendingNodes = new Stack<IBehaviorNode<AIContext>>(nodes.Reverse());
        }

        private bool HasProcessedAllNodes => pendingNodes.Count == 0;

        public string Name { get; set; } = string.Empty;

        public IEnumerable<IBehaviorNode<AIContext>> SubNodes
        { get { return nodes; } }

        public BehaviorResult Behave(AIContext context)
        {
            if (HasProcessedAllNodes)
            {
                return InvokeAndReturn();
            }

            var currentNode = pendingNodes.Pop();

            switch (behaviorResult = currentNode.Behave(context))
            {
                case BehaviorResult.Success:
                    Behave(context);
                    break;
                case BehaviorResult.Running:
                    //reevaluate the next iteration
                    pendingNodes.Push(currentNode);
                    return InvokeAndReturn();
                case BehaviorResult.Failure:
                    //stop iterating nodes
                    pendingNodes.Clear();
                    break;
            }
            return InvokeAndReturn();
        }

        private BehaviorResult InvokeAndReturn()
        {
            OnBehaved?.Invoke(this, new BehaviorInvokationEventArgs(behaviorResult));
            return behaviorResult;
        }
    }
}
