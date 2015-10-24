using Gem.Engine.AI.BehaviorTree.Composites;
using Gem.Engine.AI.BehaviorTree.Decorators;
using Gem.Engine.AI.BehaviorTree.Leaves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Engine.AI.BehaviorTree
{
    public class BehaviorTreeBuilder<AIContext>
    {
        private readonly Stack<Group> cachedNodes = new Stack<Group>();
        private Decorator<AIContext> decorator = node => node;
        
        public BehaviorTreeBuilder()
        {
            cachedNodes.Push(new Group((node, group) => { }));
        }

        private class Group
        {
            public Group(Action<IEnumerable<IBehaviorNode<AIContext>>, Group> groupEndCallback)
            {
                GroupEndCallback = groupEndCallback;
            }

            public List<IBehaviorNode<AIContext>> Nodes { get; } = new List<IBehaviorNode<AIContext>>();

            public Action<IEnumerable<IBehaviorNode<AIContext>>,Group> GroupEndCallback { get; }
        }

        public BehaviorTreeBuilder<AIContext> Behavior(
            Func<AIContext, BehaviorResult> processedBehavior,
            Func<AIContext, BehaviorResult> initialBehavior = null)
        {
            Add(decorator(
            (initialBehavior == null) ?
            new Behavior<AIContext>(processedBehavior) :
            new Behavior<AIContext>(processedBehavior, initialBehavior)));

            return this;
        }

        private void Add(IBehaviorNode<AIContext> node)
        {
            var group = cachedNodes.Pop();

            group.Nodes.Add(node);
            cachedNodes.Push(group);
        }

        public BehaviorTreeBuilder<AIContext> End
        {
            get
            {
                var group = cachedNodes.Pop();
                var previousGroup = cachedNodes.Pop();
                cachedNodes.Push(previousGroup);
                group.GroupEndCallback(group.Nodes,previousGroup);
                return this;
            }
        }

        public BehaviorTreeBuilder<AIContext> Decorate(Decorator<AIContext> newDecorator)
        {
            decorator = node =>
            {
                decorator = n => n;
                return newDecorator(node);
            };
            return this;
        }

        public BehaviorTreeBuilder<AIContext> Question(Predicate<AIContext> behaviorTest)
        {
            Add(decorator(new Question<AIContext>(behaviorTest)));
            return this;
        }

        public BehaviorTreeBuilder<AIContext> Sequence
        {
            get
            {
                var group = new Group((nodes, grp) =>
                {
                    grp.Nodes.Add(decorator(new Sequence<AIContext>(nodes.ToArray())));
                });
                cachedNodes.Push(group);
                return this;
            }
        }

        public BehaviorTreeBuilder<AIContext> Selector
        {
            get
            {
                var group = new Group((nodes,grp) =>
                {
                    grp.Nodes.Add(decorator(new Selector<AIContext>(nodes.ToArray())));
                });
                cachedNodes.Push(group);
                return this;
            }
        }

        public IBehaviorNode<AIContext> Tree
        {
            get
            {
                if (cachedNodes.Count != 1 )
                {
                    throw new Exception("Root should contain only one node");
                }
                return cachedNodes.Pop().Nodes[0];
            }
        }
    }
}
