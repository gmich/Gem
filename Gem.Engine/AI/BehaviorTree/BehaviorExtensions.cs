using System;
using Gem.AI.BehaviorTree.Decorators;

namespace Gem.AI.BehaviorTree
{
    public static class BehaviorExtensions
    {
        /// <summary>
        /// For debuggin purposes
        /// </summary>
        public static IBehaviorNode<AIContext> TraceAs<AIContext>(this IBehaviorNode<AIContext> node,string debugInfo, Action<string> debugTarget = null)
        {
            return new DebugDecorator<AIContext>(node, debugTarget ?? Console.WriteLine, debugInfo);
        }
    }
}
