using System;
using System.Collections.Generic;

namespace Gem.AI.BehaviorTree.Decorators
{
    public class DebugDecorator<AIContext> : IDecorator<AIContext>
    {
        private readonly IBehaviorNode<AIContext> decoratedNode;
        private readonly string debugName;
        private readonly Action<string> writeDebugInfo;
        private BehaviorResult behaviorResult;
        public event EventHandler OnBehaved;

        public DebugDecorator(IBehaviorNode<AIContext> decoratedNode,Action<string> writeDebugInfo, string nodeName)
        {
            this.writeDebugInfo = writeDebugInfo;
            this.decoratedNode = decoratedNode;
            debugName = nodeName;
        }

        public string Name { get; set; } = string.Empty;

        public IEnumerable<IBehaviorNode<AIContext>> SubNodes
        { get { yield return decoratedNode; } }

        public BehaviorResult Behave(AIContext context)
        {
            writeDebugInfo(Environment.NewLine +"Invoking " + debugName);
            behaviorResult = decoratedNode.Behave(context);
            writeDebugInfo(debugName + " execution terminated: " + behaviorResult) ;

            OnBehaved?.Invoke(this, new BehaviorInvokationEventArgs(behaviorResult));
            return behaviorResult;
        }
    }
}
