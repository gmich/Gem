using System;

namespace Gem.AI.BehaviorTree.Decorators
{
    public class DebugDecorator<AIContext> : IBehaviorNode<AIContext>
    {
        private readonly IBehaviorNode<AIContext> decoratedNode;
        private readonly string debugName;
        private readonly Action<string> writeDebugInfo;
        private BehaviorResult behaviorResult;

        public DebugDecorator(IBehaviorNode<AIContext> decoratedNode,Action<string> writeDebugInfo, string nodeName)
        {
            this.writeDebugInfo = writeDebugInfo;
            this.debugName = nodeName;
            this.decoratedNode = decoratedNode;
        }
        
        public BehaviorResult Behave(AIContext context)
        {
            writeDebugInfo(Environment.NewLine +"Invoking " + debugName);
            behaviorResult = decoratedNode.Behave(context);
            writeDebugInfo(debugName + " execution terminated: " + behaviorResult) ;

            return behaviorResult;
        }
    }
}
