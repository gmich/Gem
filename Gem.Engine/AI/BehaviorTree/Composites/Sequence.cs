﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Gem.AI.BehaviorTree.Composites
{
    /// <summary>
    /// Iterates BehaviorNodes terminates when a node succeeds. Behaves like the logical OR operator
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
                OnBehaved?.Invoke(this, new BehaviorInvokationEventArgs(behaviorResult));
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
            OnBehaved?.Invoke(this, new BehaviorInvokationEventArgs(BehaviorResult.Running));
            return BehaviorResult.Running;
        }
    }
}