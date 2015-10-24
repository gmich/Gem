using System;
using System.Collections.Generic;
using System.Linq;

namespace Gem.Engine.AI.BehaviorTree.Composites
{
    /// <summary>
    /// Selector. Iterates BehaviorNodes in sequence and terminates upon failure. Behaves like the logical AND operator
    /// </summary>
    /// <typeparam name="AIContext">The context to act upon</typeparam>
    public class Selector<AIContext> :  IComposite<AIContext>
    {
        private readonly IBehaviorNode<AIContext>[] nodes;
        private Stack<IBehaviorNode<AIContext>> pendingNodes;
        private BehaviorResult behaviorResult;
        public event EventHandler OnBehaved;

        public Selector(IBehaviorNode<AIContext>[] nodes)
        {
            this.nodes = nodes;
            pendingNodes = new Stack<IBehaviorNode<AIContext>>(Enumerable.Reverse(nodes));
        }

        public void Reset()
        {
            pendingNodes = new Stack<IBehaviorNode<AIContext>>(nodes.Reverse());
        }

        public string Name { get; set; } = string.Empty;

        public IEnumerable<IBehaviorNode<AIContext>> SubNodes
        { get { return nodes; } }

        private bool HasProcessedAllNodes => pendingNodes.Count == 0;

        public BehaviorResult Behave(AIContext context)
        {
            if (HasProcessedAllNodes)
            {
                return InvokeAndReturn();
            }

            var currentNode = pendingNodes.Pop();

            switch (behaviorResult = currentNode.Behave(context))
            {
                case BehaviorResult.Failure:
                    Behave(context);
                    break;
                case BehaviorResult.Running:
                    //reevaluate the next iteration
                    pendingNodes.Push(currentNode);
                    return InvokeAndReturn();
                case BehaviorResult.Success:
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
