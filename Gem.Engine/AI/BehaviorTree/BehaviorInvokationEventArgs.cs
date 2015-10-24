using System;

namespace Gem.Engine.AI.BehaviorTree
{
    public class BehaviorInvokationEventArgs : EventArgs
    {
        public BehaviorInvokationEventArgs(BehaviorResult result)
        {
            Result = result;
        }

        public BehaviorResult Result { get; }
    }
}
